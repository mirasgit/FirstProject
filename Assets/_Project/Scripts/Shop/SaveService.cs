using System;
using UnityEngine;

namespace FirstProject.Shop
{
    public class SaveService
    {
        private SaveData _data;
        private UpgradeConfig _upgradeConfig;

        private const string SAVE_KEY = "MyGameSave";

        public event Action DataChanged;

        public SaveService(UpgradeConfig upgradeConfig)
        {
            _upgradeConfig = upgradeConfig;
            Load();
        }

        public int GetUpgradeCost(UpgradeType type)
        {
            int currentLevel = 0;
            int cost = 0;

            switch (type)
            {
                case UpgradeType.Health:
                    currentLevel = _data.HealthMultiplier;
                    cost = _upgradeConfig.GetHealthCost(currentLevel);
                    break;

                case UpgradeType.Damage:
                    currentLevel = _data.DamageMultiplier;
                    cost = _upgradeConfig.GetDamageCost(currentLevel);
                    break;

                case UpgradeType.AttackSpeed:
                    currentLevel = _data.AttackSpeedMultiplier;
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
                    _data.HealthMultiplier++;
                    break;

                case UpgradeType.Damage:
                    _data.DamageMultiplier++;
                    break;

                case UpgradeType.AttackSpeed:
                    _data.AttackSpeedMultiplier++;
                    break;
            }
            Save();
            return true;
        }

        public int GetUpgradeBaseCost(UpgradeType type)
        {
            switch (type)
            {
                case UpgradeType.Health:
                    return _upgradeConfig.GetHealthCost(_data.HealthMultiplier);
                case UpgradeType.Damage:
                    return _upgradeConfig.GetDamageCost(_data.DamageMultiplier);
                case UpgradeType.AttackSpeed:
                    return _upgradeConfig.GetAttackSpeedCost(_data.AttackSpeedMultiplier);
            }
            return 0;
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string jsonString = PlayerPrefs.GetString(SAVE_KEY);

                _data = JsonUtility.FromJson<SaveData>(jsonString);
            }
            else
            {
                _data = new SaveData();
            }
        }

        public void Save()
        {
            string jsonString = JsonUtility.ToJson( _data);

            PlayerPrefs.SetString(SAVE_KEY, jsonString);

            PlayerPrefs.Save();
            DataChanged?.Invoke();
        }

        public void AddCoins(int amount)
        {
            _data.Coins += amount;
            Save();
        }

        public int GetCoins()
        {
            return _data.Coins;
        }
        
        public int GetHealthLVL()
        {
            return _data.HealthMultiplier;
        }

        public float GetHealthMP()
        {
            return _data.HealthMultiplier * _upgradeConfig.GetHealthMultiplier();
        }

        public int GetDamageLVL()
        {
            return _data.DamageMultiplier;
        }

        public float GetDamageMP()
        {
            return _data.DamageMultiplier * _upgradeConfig.GetDamageMultiplier();
        }

        public int GetAttackSpeedLVL()
        {
            return _data.AttackSpeedMultiplier;
        }

        public float GetAttackSpeedMP()
        {
            return _data.AttackSpeedMultiplier * _upgradeConfig.GetAttackSpeedMultiplier();
        }
    }
}