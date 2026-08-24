using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstProject.Shop.UI
{
    public class ShopView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _upgradeHealthButton;
        [SerializeField] private Button _upgradeDamageButton;

        [Header("Counters")]
        [SerializeField] private Button _upgradeAttackSpeedButton;
        [SerializeField] private TextMeshProUGUI _coinCounter;
        [SerializeField] private TextMeshProUGUI _healthLevelCounter;
        [SerializeField] private TextMeshProUGUI _damageLevelCounter;
        [SerializeField] private TextMeshProUGUI _attackSpeedLevelCounter;
        [SerializeField] private TextMeshProUGUI _healthLevelCost;
        [SerializeField] private TextMeshProUGUI _damageLevelCost;
        [SerializeField] private TextMeshProUGUI _attackSpeedLevelCost;

        [Header("Panels")]
        [SerializeField] private GameObject _shopEntryPanel;
        [SerializeField] private GameObject _shopPanel;

        public event Action ShopButtonClicked;
        public event Action BackButtonClicked;
        public event Action HealthUpgradeButtonClicked;
        public event Action DamageUpgradeButtonClicked;
        public event Action AttackSpeedUpgradeButtonClicked;

        public void Subscribe()
        {
            _shopButton.onClick.AddListener(OnShopButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _upgradeHealthButton.onClick.AddListener(OnHealthUpgradeButtonClicked);
            _upgradeDamageButton.onClick.AddListener(OnDamageUpgradeButtonClicked);
            _upgradeAttackSpeedButton.onClick.AddListener(OnAttackSpeedUpgradeButtonClicked);
        }

        public void Unsubscribe()
        {
            _shopButton.onClick.RemoveListener(OnShopButtonClicked);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _upgradeHealthButton.onClick.RemoveListener(OnHealthUpgradeButtonClicked);
            _upgradeDamageButton.onClick.RemoveListener(OnDamageUpgradeButtonClicked);
            _upgradeAttackSpeedButton.onClick.RemoveListener(OnAttackSpeedUpgradeButtonClicked);
        }

        public void UpdateData(int coinCount,int healthLVL, int damageLVL, int attackSpeedLVL)
        {
            _coinCounter.text = coinCount.ToString();
            _healthLevelCounter.text = healthLVL.ToString();
            _damageLevelCounter.text = damageLVL.ToString();
            _attackSpeedLevelCounter.text = attackSpeedLVL.ToString();
        }

        public void UpdateCosts(int healthCost, int damageCost, int attackSpeedCost)
        {
            _healthLevelCost.text = healthCost.ToString();
            _damageLevelCost.text = damageCost.ToString();
            _attackSpeedLevelCost.text = attackSpeedCost.ToString();
        }

        public void HideShop()
        {
            _shopPanel.SetActive(false);
            _shopEntryPanel.SetActive(false);
        }

        public void ShowShopPanel()
        {
            _shopPanel.SetActive(true);
            _shopEntryPanel.SetActive(false);
        }

        public void ShowShopEntryPanel()
        {
            _shopPanel.SetActive(false);
            _shopEntryPanel.SetActive(true);
        }

        private void OnShopButtonClicked()
        {
            ShopButtonClicked?.Invoke();
        }

        private void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }

        private void OnHealthUpgradeButtonClicked()
        {
            HealthUpgradeButtonClicked?.Invoke();
        }

        private void OnDamageUpgradeButtonClicked()
        {
            DamageUpgradeButtonClicked?.Invoke();
        }

        private void OnAttackSpeedUpgradeButtonClicked()
        {
            AttackSpeedUpgradeButtonClicked?.Invoke();
        }
    }
}