using Zenject;
using System;
using FirstProject.Battle;

namespace FirstProject.Shop
{
    public class ShopPresenter : IInitializable, IDisposable
    {
        private readonly ShopView _view;
        private readonly ProgressModel _model;
        private readonly BattleFlow _battleFlow;
        public ShopPresenter(ProgressModel model, ShopView view, BattleFlow battleFlow)
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
            _view.UpdateData(_model.Coins, _model.HealthLevel, _model.DamageLevel, _model.AttackSpeedLevel);
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
            _battleFlow.ShowShopScreen();
            _view.ShowShopPanel();
        }

        private void OnBackButtonClicked()
        {
            _battleFlow.HideShopScreen();
            _view.ShowShopEntryPanel();
        }
    }
}