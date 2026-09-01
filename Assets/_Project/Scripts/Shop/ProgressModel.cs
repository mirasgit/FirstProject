using System;
using FirstProject.Configs;

namespace FirstProject.Shop
{
    public class ProgressModel : Zenject.IInitializable, IDisposable
    {
        private readonly ISaveService _saveService;
        private readonly RemoteConfigService _configService;
        private SaveData _data;

        public int Coins => _data.Coins;

        public int HealthLevel => _data.HealthLevel;

        public float HealthMultiplier => _data.HealthLevel * _configService.Data.Upgrades.HealthMultiplierPerPurchase;

        public int DamageLevel => _data.DamageLevel;

        public float DamageMultiplier => _data.DamageLevel * _configService.Data.Upgrades.DamageMultiplierPerPurchase;

        public int AttackSpeedLevel => _data.AttackSpeedLevel;

        public float AttackSpeedMultiplier => _data.AttackSpeedLevel * _configService.Data.Upgrades.AttackSpeedMultiplierPerPurchase;


        public event Action DataChanged;

        public ProgressModel(ISaveService saveService, RemoteConfigService configService)
        {
            _saveService = saveService;
            _configService = configService;
            _data = _saveService.Load(); 
        }

        public void Initialize()
        {
            _configService.OnConfigLoaded += TriggerDataChanged;
        }

        public void Dispose()
        {
            _configService.OnConfigLoaded -= TriggerDataChanged;
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
            if (_data.Coins < GetUpgradeCost(upgradeType))
            {
                return false;
            }

            _data.Coins -= GetUpgradeCost(upgradeType);

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
        }

        private void TriggerDataChanged()
        {
            DataChanged?.Invoke();
        }
    }
}
