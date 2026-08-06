using UnityEngine;
namespace FirstProject.Shop
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Shop/Upgrade Config")]
    public class UpgradeConfig : ScriptableObject
    {
        [Header("Prices")]
        [SerializeField] private int _baseHealthCost = 100;
        [SerializeField] private int _healthCostStep = 20;
        [SerializeField] private int _baseDamageCost = 100;
        [SerializeField] private int _damageCostStep = 30;
        [SerializeField] private int _baseAttackSpeedCost = 100;
        [SerializeField] private int _attackSpeedCostStep = 40;

        [Header("Multipliers")]
        [SerializeField] private float _healthMultiplierPerPurchase = 0.1f;
        [SerializeField] private float _damageMultiplierPerPurchase = 0.1f;
        [SerializeField] private float _attackSpeedMultiplierPerPurchase = 0.1f;

        public int GetHealthCost(int currentLevel)
        {
            return _baseHealthCost + (_healthCostStep * currentLevel);
        }

        public int GetDamageCost(int currentLevel)
        {
            return _baseDamageCost + (_damageCostStep * currentLevel);
        }

        public int GetAttackSpeedCost(int currentLevel)
        {
            return _baseAttackSpeedCost + (_attackSpeedCostStep * currentLevel);
        }

        public float GetHealthMultiplier()
        {
            return _healthMultiplierPerPurchase;
        }

        public float GetDamageMultiplier()
        {
            return _damageMultiplierPerPurchase;
        }

        public float GetAttackSpeedMultiplier()
        {
            return _attackSpeedMultiplierPerPurchase;
        }
    }
}