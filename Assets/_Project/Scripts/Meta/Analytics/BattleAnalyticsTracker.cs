using Zenject;
using System;
using FirstProject.Core.Battle;
using FirstProject.Meta.Multiplayer;

namespace FirstProject.Meta.Analytics
{
    public class BattleAnalyticsTracker : IInitializable, IDisposable
    {
        private readonly BattleFlow _battleFlow;
        private readonly IAnalyticsService _analytics;
        private readonly IMultiplayerService _multiplayerService;

        public BattleAnalyticsTracker(BattleFlow battleFlow, IAnalyticsService analytics, IMultiplayerService multiplayerService)
        {
            _battleFlow = battleFlow;
            _analytics = analytics;
            _multiplayerService = multiplayerService;
        }

        public void Initialize()
        {
            _battleFlow.WinnerDecided += OnWinnerDecided;
        }

        public void Dispose()
        {
            _battleFlow.WinnerDecided -= OnWinnerDecided;
        }

        private void OnWinnerDecided(BattleResult result)
        {
            
            if (_multiplayerService.InRoom)
            {
                BattleResult localWinner = _multiplayerService.IsMasterClient ? BattleResult.LeftWon : BattleResult.RightWon;

                if (result == localWinner)
                {
                    _analytics.LogEvent("battle_won");
                }
                else
                {
                    _analytics.LogEvent("battle_lost");
                }
            }
            else
            {
                if (result == BattleResult.RightWon)
                {
                    _analytics.LogEvent("battle_lost");
                }
                else if (result == BattleResult.LeftWon)
                {
                    _analytics.LogEvent("battle_won");
                }
            }
        }
    }
}