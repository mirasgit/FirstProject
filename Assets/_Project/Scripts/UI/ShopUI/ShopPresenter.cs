using Zenject;
using System;
using FirstProject.Battle;

namespace FirstProject.Shop.UI
{
    public class ShopPresenter : IInitializable, IDisposable
    {
        private readonly ShopView _view;
        private readonly ProgressModel _progressModel;
        private readonly BattleFlow _battleFlow;
        public ShopPresenter(ProgressModel model, ShopView view, BattleFlow battleFlow)
        {
            _progressModel = model;
            _view = view;
            _battleFlow = battleFlow;
        }

        public void Initialize()
        {
            _battleFlow.BattleStarted += OnBattleStarted;
            _battleFlow.RoundFinished += OnRoundFinished;
            _battleFlow.StartScreenShowed += OnStartScreenShowed;
            _view.Subscribe();
            _view.ShopButtonClicked += OnShopButtonClicked;
            _view.BackButtonClicked += OnBackButtonClicked;
            _view.HealthUpgradeButtonClicked += OnHealthUpgradeButtonClicked;
            _view.DamageUpgradeButtonClicked += OnDamageUpgradeButtonClicked;
            _view.AttackSpeedUpgradeButtonClicked += OnAttackSpeedUpgradeButtonClicked;
            _progressModel.DataChanged += OnDataChanged;

            _view.HideShop();
        }

        public void Dispose()
        {
            _battleFlow.BattleStarted -= OnBattleStarted;
            _battleFlow.RoundFinished -= OnRoundFinished;
            _battleFlow.StartScreenShowed -= OnStartScreenShowed;
            _view.Unsubscribe();
            _view.ShopButtonClicked -= OnShopButtonClicked;
            _view.BackButtonClicked -= OnBackButtonClicked;
            _view.HealthUpgradeButtonClicked -= OnHealthUpgradeButtonClicked;
            _view.DamageUpgradeButtonClicked -= OnDamageUpgradeButtonClicked;
            _view.AttackSpeedUpgradeButtonClicked -= OnAttackSpeedUpgradeButtonClicked;
            _progressModel.DataChanged -= OnDataChanged;
        }

        private void OnStartScreenShowed()
        {
            _view.ShowShopEntryPanel();
            OnDataChanged();
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
            _view.UpdateData(_progressModel.Coins, _progressModel.HealthLevel, _progressModel.DamageLevel, _progressModel.AttackSpeedLevel);
            _view.UpdateCosts(_progressModel.GetUpgradeCost(UpgradeType.Health), _progressModel.GetUpgradeCost(UpgradeType.Damage), _progressModel.GetUpgradeCost(UpgradeType.AttackSpeed));
        }

        private void OnHealthUpgradeButtonClicked()
        {
            _progressModel.TryUpgrade(UpgradeType.Health);
        }

        private void OnDamageUpgradeButtonClicked()
        {
            _progressModel.TryUpgrade(UpgradeType.Damage);
        }

        private void OnAttackSpeedUpgradeButtonClicked()
        {
            _progressModel.TryUpgrade(UpgradeType.AttackSpeed);
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