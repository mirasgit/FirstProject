using System;

namespace FirstProject.Shop
{
    public class ProgressModel
    {
        private readonly ISaveService _saveService;
        private readonly UpgradeConfig _upgradeConfig;
        private SaveData _data;

        public int Coins => _data.Coins;

        public int HealthLevel => _data.HealthLevel;

        public float HealthMultiplier => (_data.HealthLevel * _upgradeConfig.GetHealthMultiplier());

        public int DamageLevel => _data.DamageLevel;

        public float DamageMultiplier => _data.DamageLevel * _upgradeConfig.GetDamageMultiplier();

        public int AttackSpeedLevel => _data.AttackSpeedLevel;

        public float AttackSpeedMultiplier => _data.AttackSpeedLevel * _upgradeConfig.GetAttackSpeedMultiplier();


        public event Action DataChanged;

        public ProgressModel(ISaveService saveService, UpgradeConfig upgradeConfig)
        {
            _saveService = saveService;
            _upgradeConfig = upgradeConfig;
            _data = _saveService.Load(); 
        }

        public int GetUpgradeCost(UpgradeType type)
        {
            int currentLevel = 0;
            int cost = 0;

            switch (type)
            {
                case UpgradeType.Health:
                    currentLevel = _data.HealthLevel;
                    cost = _upgradeConfig.GetHealthCost(currentLevel);
                    break;

                case UpgradeType.Damage:
                    currentLevel = _data.DamageLevel;
                    cost = _upgradeConfig.GetDamageCost(currentLevel);
                    break;

                case UpgradeType.AttackSpeed:
                    currentLevel = _data.AttackSpeedLevel;
                    cost = _upgradeConfig.GetAttackSpeedCost(currentLevel);
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
    }
}
