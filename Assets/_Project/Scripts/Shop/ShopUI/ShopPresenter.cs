using Zenject;
using FirstProject.UI;
using System;
using FirstProject.Battle;

namespace FirstProject.Shop
{
    public class ShopPresenter : IInitializable, IDisposable
    {
        private ShopView _view;
        private SaveService _model;
        private BattleFlow _battleFlow;
        public ShopPresenter(SaveService model, ShopView view, BattleFlow battleFlow)
        {
            _model = model;
            _view = view;
            _battleFlow = battleFlow;
        }

        public void Initialize()
        {
            _battleFlow.BattleStarted += OnBattleStarted;
            _battleFlow.RoundFinished += OnRoundFinished;
            _view.Subscribe();
            _view.ShopButtonClicked += OnShopButtonClicked;
            _view.BackButtonClicked += OnBackButtonClicked;
            _view.HealthUpgradeButtonClicked += OnHealthUpgradeButtonClicked;
            _view.DamageUpgradeButtonClicked += OnDamageUpgradeButtonClicked;
            _view.AttackSpeedUpgradeButtonClicked += OnAttackSpeedUpgradeButtonClicked;
            _model.DataChanged += OnDataChanged;

            _view.ShowShopEntryPanel();
            OnDataChanged();
        }

        public void Dispose()
        {
            _battleFlow.BattleStarted -= OnBattleStarted;
            _battleFlow.RoundFinished -= OnRoundFinished;
            _view.Unsubscribe();
            _view.ShopButtonClicked -= OnShopButtonClicked;
            _view.BackButtonClicked -= OnBackButtonClicked;
            _view.HealthUpgradeButtonClicked -= OnHealthUpgradeButtonClicked;
            _view.DamageUpgradeButtonClicked -= OnDamageUpgradeButtonClicked;
            _view.AttackSpeedUpgradeButtonClicked -= OnAttackSpeedUpgradeButtonClicked;
            _model.DataChanged -= OnDataChanged;
        }

        private void OnRoundFinished()
        {
            _view.ShowShopEntryPanel();
        }

        private void OnBattleStarted()
        {
            _view.HideShop();
        }
        private void OnDataChanged()
        {
            _view.UpdateData(_model.GetCoins(), _model.GetHealthLVL(), _model.GetDamageLVL(), _model.GetAttackSpeedLVL());
            _view.UpdateCosts(_model.GetUpgradeCost(UpgradeType.Health), _model.GetUpgradeCost(UpgradeType.Damage), _model.GetUpgradeCost(UpgradeType.AttackSpeed));
        }

        private void OnHealthUpgradeButtonClicked()
        {
            _model.TryUpgrade(UpgradeType.Health);
        }

        private void OnDamageUpgradeButtonClicked()
        {
            _model.TryUpgrade(UpgradeType.Damage);
        }

        private void OnAttackSpeedUpgradeButtonClicked()
        {
            _model.TryUpgrade(UpgradeType.AttackSpeed);
        }

        private void OnShopButtonClicked()
        {
            _view.ShowShopPanel();
        }

        private void OnBackButtonClicked()
        {
            _view.ShowShopEntryPanel();
        }
    }
}