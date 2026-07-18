using FirstProject.Battle;
using System;

namespace FirstProject.UI
{
    public class BattlePresenter : Zenject.IInitializable, IDisposable
    {
        private readonly BattleView _view;
        private readonly BattleFlow _model;

        private const string LeftWonText = "Left Won";
        private const string RightWonText = "Right Won";

        public BattlePresenter(BattleView view, BattleFlow model)
        {   
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            _model.BattleStarted += OnBattleStarted;
            _model.StartScreenShowed += OnStartScreenShowed;
            _model.WhoWon += OnWinnerDecided;
        }

        public void Dispose()
        {
            _model.BattleStarted -= OnBattleStarted;
            _model.StartScreenShowed -= OnStartScreenShowed;
            _model.WhoWon -= OnWinnerDecided;
        }

        private void OnBattleStarted()
        {
            _view.ShowBattleScreen();
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
                    _view.ShowWinner(LeftWonText);
                    break;
                case BattleResult.RightWon:
                    _view.ShowWinner(RightWonText);
                    break;
            }
        }
    }
}