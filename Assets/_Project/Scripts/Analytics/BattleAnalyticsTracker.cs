using UnityEngine;
using Zenject;
using FirstProject.Battle;
using System;

namespace FirstProject.Analytics
{
    public class BattleAnalyticsTracker : IInitializable, IDisposable
    {
        private readonly BattleFlow _battleFlow;
        private readonly IAnalyticsService _analytics;

        public BattleAnalyticsTracker(BattleFlow battleFlow, IAnalyticsService analytics)
        {
            _battleFlow = battleFlow;
            _analytics = analytics;
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