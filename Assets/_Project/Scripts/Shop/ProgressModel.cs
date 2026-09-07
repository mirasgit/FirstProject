    using System;
    using System.Collections.Generic;
    using FirstProject.Analytics;
    using FirstProject.Configs;

    namespace FirstProject.Shop
    {
        public class ProgressModel
        {
            private readonly ISaveService _saveService;
            private readonly IRemoteConfigService _configService;
            private readonly IAnalyticsService _analyticsService;
            private SaveData _data;

            public int Coins => _data.Coins;

            public int HealthLevel => _data.HealthLevel;

            public float HealthMultiplier => _data.HealthLevel * _configService.Data.Upgrades.HealthMultiplierPerPurchase;

            public int DamageLevel => _data.DamageLevel;

            public float DamageMultiplier => _data.DamageLevel * _configService.Data.Upgrades.DamageMultiplierPerPurchase;

            public int AttackSpeedLevel => _data.AttackSpeedLevel;

            public float AttackSpeedMultiplier => _data.AttackSpeedLevel * _configService.Data.Upgrades.AttackSpeedMultiplierPerPurchase;

            public bool isAdsRemoved => _data.IsAdsRemoved;

            public event Action DataChanged;

            public ProgressModel(ISaveService saveService, IRemoteConfigService configService, IAnalyticsService analyticsService)
            {
                _saveService = saveService;
                _configService = configService;
                _data = _saveService.Load();
                _analyticsService = analyticsService;
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
                _saveService.Save(_data);
                DataChanged?.Invoke();
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
        }
    }
