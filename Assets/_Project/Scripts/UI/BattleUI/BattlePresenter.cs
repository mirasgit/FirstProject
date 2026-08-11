using FirstProject.Battle;
using System;
using Zenject;

namespace FirstProject.UI
{
    public class BattlePresenter : IInitializable, IDisposable
    {
        private readonly BattleView _view;
        private readonly BattleFlow _model;

        private const string LEFT_WON_TEXT = "Left Won";
        private const string RIGHT_WON_TEXT  = "Right Won";

        public BattlePresenter(BattleView view, BattleFlow model)
        {   
            _view = view;
            _model = model;
        }


        public void Initialize()
        {
            _model.BattleStarted += OnBattleStarted;
            _model.StartScreenShowed += OnStartScreenShowed;
            _model.WinnerDecided += OnWinnerDecided;
            _model.ShopScreenShowed += OnShopScreenShowed;
            _model.WinScreenShowed += OnWinScreenShowed;
            _view.StartButtonPressed += OnStartButtonPressed;
            _view.RestartButtonPressed += OnRestartButtonPressed;
            _view.ExitButtonPressed += OnExitButtonPressed;
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
            _model.BattleStarted -= OnBattleStarted;
            _model.StartScreenShowed -= OnStartScreenShowed;
            _model.WinnerDecided -= OnWinnerDecided;
            _model.ShopScreenShowed -= OnShopScreenShowed;
            _model.WinScreenShowed -= OnWinScreenShowed;
            _view.StartButtonPressed -= OnStartButtonPressed;
            _view.RestartButtonPressed -= OnRestartButtonPressed;
            _view.ExitButtonPressed -= OnExitButtonPressed;
            _view.Unsubscribe();
        }

        private void OnWinScreenShowed()
        {
            OnWinnerDecided(_model.LastWinner);
        }

        private void OnShopScreenShowed()
        {
            _view.HideScreens();
        }

        private void OnStartButtonPressed()
        {
            _model.StartBattle();
        }
        private void OnRestartButtonPressed()
        {
            _model.RestartBattle();
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
                    _view.ShowWinner(LEFT_WON_TEXT);
                    break;
                case BattleResult.RightWon:
                    _view.ShowWinner(RIGHT_WON_TEXT);
                    break;
            }
        }
    }
}