using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using FirstProject.Core.Battle;
using FirstProject.Core.SceneLoading;
using FirstProject.Meta.Ads;
using FirstProject.Meta.Multiplayer;
using Photon.Pun;
using System;
using System.Threading;
using Zenject;

namespace FirstProject.UI.Battle
{
    public class BattlePresenter : IInitializable, IDisposable
    {
        private readonly BattleView _view;
        private readonly BattleFlow _model;
        private readonly IAdsService _adsService;
        private CancellationTokenSource _battleCts = new CancellationTokenSource();
        private readonly ISceneLoadService _sceneLoader;
        private readonly MatchStateService _matchStateService;

        private const string LEFT_WON_TEXT = "Left Won";
        private const string RIGHT_WON_TEXT = "Right Won";

        private bool _isRewardClaimed;

        public BattlePresenter(BattleView view, BattleFlow model, IAdsService adsService, ISceneLoadService sceneLoader, MatchStateService matchStateService)
        {   
            _view = view;
            _model = model;
            _adsService = adsService;
            _sceneLoader = sceneLoader;
            _matchStateService = matchStateService;
        }

        public void Initialize()
        {
            _matchStateService.WaitingForOpponent += OnWaitingForOpponent;
            _matchStateService.AllPlayersReady += OnAllPlayersReady;

            _model.BattleStarted += OnBattleStarted;
            _model.StartScreenShowed += OnStartScreenShowed;
            _model.WinnerDecided += OnWinnerDecided;
            _model.ShopOpened += OnShopOpened;
            _model.ShopClosed += OnShopClosed;
            _view.StartButtonPressed += OnStartButtonPressed;
            _view.RestartButtonPressed += OnRestartButtonPressed;
            _view.ExitButtonPressed += OnExitButtonPressed;
            _view.RewardButtonPressed += OnRewardButtonPressed;
            _view.ReadyButtonPressed += OnReadyButtonPressed;
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

            _matchStateService.WaitingForOpponent -= OnWaitingForOpponent;
            _matchStateService.AllPlayersReady -= OnAllPlayersReady;

            _model.BattleStarted -= OnBattleStarted;
            _model.StartScreenShowed -= OnStartScreenShowed;
            _model.WinnerDecided -= OnWinnerDecided;
            _model.ShopOpened -= OnShopOpened;
            _model.ShopClosed -= OnShopClosed;
            _view.StartButtonPressed -= OnStartButtonPressed;
            _view.RestartButtonPressed -= OnRestartButtonPressed;
            _view.ExitButtonPressed -= OnExitButtonPressed;
            _view.RewardButtonPressed -= OnRewardButtonPressed;
            _view.ReadyButtonPressed -= OnReadyButtonPressed;
            _view.Unsubscribe();
        }

        private void OnAllPlayersReady()
        {
            var hash = new Hashtable { { "IsReady", false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            UnityEngine.Debug.Log("hash is: " +  hash);

            _view.SetReadyButtonInteractable(true);

            if (_model.State == BattleState.Finished)
            {
                _model.RestartBattle(_battleCts.Token);
            }
            else
            {
                _model.StartBattleAsync(_battleCts.Token).Forget();
            }
        }

        private void OnWaitingForOpponent(bool waiting)
        {
            _view.ShowLoadingScreen(waiting);
            _view.ShowReadyScreen(!waiting);
        }

        private void OnShopOpened()
        {
            _view.HideScreens();
        }

        private void OnShopClosed()
        {
            if (_model.State == BattleState.StartScreen)
            {
                _view.ShowStartScreen(true);
            }
            else if (_model.State == BattleState.Finished)
            {
                string winnerText = _model.LastWinner == BattleResult.LeftWon ? LEFT_WON_TEXT : RIGHT_WON_TEXT;
                _view.ShowWinner(winnerText, !_isRewardClaimed);
            }
        }

        private void OnReadyButtonPressed()
        {
            _view.SetReadyButtonInteractable(false);
            var hash = new Hashtable { { "IsReady", true } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        private void OnRewardButtonPressed()
        {
            _adsService.ShowRewardedAd(() =>
            {
                _isRewardClaimed = true;
                _model.ClaimReward();
                _view.HideRewardButton();
            });
        }

        private void OnStartButtonPressed()
        {
            _model.StartBattleAsync(_battleCts.Token).Forget();
        }
        private void OnRestartButtonPressed()
        {
            if (PhotonNetwork.InRoom)
            {
                _view.HideScreens();
                _view.ShowReadyScreen(true);
            }
            else
            {
                _model.RestartBattle(_battleCts.Token);
            }
        }

        private void OnExitButtonPressed()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                _sceneLoader.LoadScene(SceneName.MainMenu);
            }
            else
            {
                _sceneLoader.LoadScene(SceneName.MainMenu);
            }
        }

        private void OnBattleStarted()
        {
            _view.HideScreens();
        }

        private void OnStartScreenShowed()
        {
            if (PhotonNetwork.InRoom)
            {
                _view.HideScreens();
                _view.ShowReadyScreen(true);
            }
            else
            {
                _view.ShowReadyScreen(false);
                _view.ShowStartScreen(true);
            }
        }

        private void OnWinnerDecided(BattleResult result)
        {
            _isRewardClaimed = false;
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