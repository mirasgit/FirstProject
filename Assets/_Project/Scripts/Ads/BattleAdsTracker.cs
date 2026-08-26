using FirstProject.Battle;
using System;
using Zenject;

namespace FirstProject.Ads
{
    public class BattleAdsTracker : IInitializable, IDisposable
    {
        private readonly BattleFlow _battleFlow;
        private readonly IAdsService _adsService;

        private int _battleCount = 0;

        public BattleAdsTracker(BattleFlow battleFlow, IAdsService adsService)
        {
            _battleFlow = battleFlow;
            _adsService = adsService;
        }

        public void Initialize()
        {
            _battleFlow.RoundFinished += OnRoundFinished;
        }

        public void Dispose()
        {
            _battleFlow.RoundFinished -= OnRoundFinished;
        }

        private void OnRoundFinished()
        {
            _battleCount++;

            if (_battleCount % 2 == 0)
            {
                _adsService.ShowInterstitialAd();
            }
        }
    }
}