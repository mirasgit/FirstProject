using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FirstProject.Core.SaveSystem;
using FirstProject.Meta.Analytics;
using FirstProject.Meta.Configs;
using Unity.VisualScripting;

namespace FirstProject.Meta.Shop
{
    public class ProgressModel
    {
        private readonly ISaveService _saveService;
        private readonly IRemoteConfigService _configService;
        private readonly IAnalyticsService _analyticsService;
        private SaveData _data;

        private bool _isSaving;
        private bool _saveQueued;

        public int Coins => _data.Coins;

        public int HealthLevel => _data.HealthLevel;

        public float HealthMultiplier => _data.HealthLevel * _configService.Data.Upgrades.HealthMultiplierPerPurchase;

        public int DamageLevel => _data.DamageLevel;

        public float DamageMultiplier => _data.DamageLevel * _configService.Data.Upgrades.DamageMultiplierPerPurchase;

        public int AttackSpeedLevel => _data.AttackSpeedLevel;

        public float AttackSpeedMultiplier => _data.AttackSpeedLevel * _configService.Data.Upgrades.AttackSpeedMultiplierPerPurchase;

        public bool IsAdsRemoved => _data.IsAdsRemoved;

        public bool IsInitialized => _data != null;

        public event Action DataChanged;

        public ProgressModel(ISaveService saveService, IRemoteConfigService configService, IAnalyticsService analyticsService)
        {
            _saveService = saveService;
            _configService = configService;
            _analyticsService = analyticsService;
        }

        public async UniTask InitializeDataAsync(ISaveConflictResolver resolver, CancellationToken token = default)
        {
            if (_data != null)
            {
                return;
            }
            _data = await _saveService.LoadAsync(resolver, token);

            DataChanged?.Invoke();
        }

        public void RemoveAds()
        {
            _data.IsAdsRemoved = true;
            Save();
        }

        public int GetUpgradeCost(UpgradeType type)
        {
            int currentLevel = 0;
            int cost = 0;

            switch (type)
            {
                case UpgradeType.Health:
                    currentLevel = _data.HealthLevel;
                    cost = _configService.Data.Upgrades.BaseHealthCost + (_configService.Data.Upgrades.HealthCostStep * currentLevel);
                    break;

                case UpgradeType.Damage:
                    currentLevel = _data.DamageLevel;
                    cost = _configService.Data.Upgrades.BaseDamageCost + (_configService.Data.Upgrades.DamageCostStep * currentLevel);
                    break;
                
                case UpgradeType.AttackSpeed:
                    currentLevel = _data.AttackSpeedLevel;
                    cost = _configService.Data.Upgrades.BaseAttackSpeedCost + (_configService.Data.Upgrades.AttackSpeedCostStep * currentLevel);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, "No such type");
            }
            return cost;
        }

        public bool TryUpgrade(UpgradeType upgradeType)
        {
            int currentCost = GetUpgradeCost(upgradeType);

            if (_data.Coins < currentCost)
            {
                return false;
            }

            _data.Coins -= currentCost;

            switch (upgradeType)
            {
                case UpgradeType.Health:
                    _data.HealthLevel++;
                    break;
                case UpgradeType.Damage:
                    _data.DamageLevel++;
                    break;
                case UpgradeType.AttackSpeed:
                    _data.AttackSpeedLevel++;
                    break;
            }

            Save();

            _analyticsService.LogEvent("upgrade_purchased", new Dictionary<string, object> {
            { "upgrade_type", upgradeType.ToString() },
            { "cost", currentCost }
            });
            return true;
        }

        public void Save()
        {
            _data.LastSaveTimeTicks = DateTime.UtcNow.Ticks;
            DataChanged?.Invoke();
            ProcessSaveQueueAsync().Forget();
        }

        public void AddCoins(int amount)
        {
            _data.Coins += amount;
            Save();
            _analyticsService.LogEvent("currency_changed", new Dictionary<string, object> {
            { "amount_added", amount},
            { "total_coins", _data.Coins }
            });
        }

        private async UniTaskVoid ProcessSaveQueueAsync()
        {
            if (_isSaving)
            {
                _saveQueued = true;
                return;
            }
            
            _isSaving = true;
            _saveQueued = false;

            try
            {
                await _saveService.SaveAsync(_data);
            }
            finally
            {
                _isSaving = false;
                if (_saveQueued )
                {
                    ProcessSaveQueueAsync().Forget();
                }
            }
        }
    }
}
