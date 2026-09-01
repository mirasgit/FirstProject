using Cysharp.Threading.Tasks;
using FirstProject.Ads;
using System;
using System.Threading;
using Zenject;

namespace FirstProject.Battle.UI
{
    public class BattlePresenter : IInitializable, IDisposable
    {
        private readonly BattleView _view;
        private readonly BattleFlow _model;
        private readonly IAdsService _adsService;
        private CancellationTokenSource _battleCts = new CancellationTokenSource();

        private const string LEFT_WON_TEXT = "Left Won";
        private const string RIGHT_WON_TEXT = "Right Won";

        public BattlePresenter(BattleView view, BattleFlow model, IAdsService adsService)
        {   
            _view = view;
            _model = model;
            _adsService = adsService;
        }

        public void Initialize()
        {
            _model.BattleStarted += OnBattleStarted;
            _model.StartScreenShowed += OnStartScreenShowed;
            _model.WinnerDecided += OnWinnerDecided;
            _model.ShopOpened += OnShopOpened;
            _model.ShopClosed += OnShopClosed;
            _view.StartButtonPressed += OnStartButtonPressed;
            _view.RestartButtonPressed += OnRestartButtonPressed;
            _view.ExitButtonPressed += OnExitButtonPressed;
            _view.RewardButtonPressed += OnRewardButtonPressed;
            _view.Subscribe();

            if (_model.State == BattleState.StartScreen)
            {
                OnStartScreenShowed();
            }
            else if (_model.State == BattleState.Running)
            {
                OnBattleStarted();
            }
        }

        public void Dispose()
        {
            _battleCts.Cancel();
            _battleCts.Dispose();

            _model.BattleStarted -= OnBattleStarted;
            _model.StartScreenShowed -= OnStartScreenShowed;
            _model.WinnerDecided -= OnWinnerDecided;
            _model.ShopOpened -= OnShopOpened;
            _model.ShopClosed -= OnShopClosed;
            _view.StartButtonPressed -= OnStartButtonPressed;
            _view.RestartButtonPressed -= OnRestartButtonPressed;
            _view.ExitButtonPressed -= OnExitButtonPressed;
            _view.RewardButtonPressed -= OnRewardButtonPressed;
            _view.Unsubscribe();
        }

        private void OnRewardButtonPressed()
        {
            _view.HideRewardButton();
            _adsService.ShowRewardedAd(() =>
            {
                _model.ClaimReward();
            });
        }

        private void OnShopOpened()
        {
            _view.HideScreens();
        }

        private void OnShopClosed()
        {
            if (_model.State == BattleState.StartScreen)
            {
                _view.ShowStartScreen();
            }
            else if (_model.State == BattleState.Finished)
            {
                string winnerText = _model.LastWinner == BattleResult.LeftWon ? LEFT_WON_TEXT : RIGHT_WON_TEXT;
                _view.ShowWinner(winnerText, false);
            }
        }

        private void OnStartButtonPressed()
        {
            _model.StartBattleAsync(_battleCts.Token).Forget();
        }
        private void OnRestartButtonPressed()
        {
            _model.RestartBattle(_battleCts.Token);
        }

        private void OnExitButtonPressed()
        {
            _view.ExitGame();
        }

        private void OnBattleStarted()
        {
            _view.HideScreens();
        }

        private void OnStartScreenShowed()
        {
            _view.ShowStartScreen();
        }

        private void OnWinnerDecided(BattleResult result)
        {
            switch (result)
            {
                case BattleResult.LeftWon:
                    _view.ShowWinner(LEFT_WON_TEXT, true);
                    break;
                case BattleResult.RightWon:
                    _view.ShowWinner(RIGHT_WON_TEXT, true);
                    break;
            }
        }
    }
}