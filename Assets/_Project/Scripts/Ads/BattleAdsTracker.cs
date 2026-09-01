using FirstProject.Battle;
using FirstProject.Configs;
using System;
using Zenject;

namespace FirstProject.Ads
{
    public class BattleAdsTracker : IInitializable, IDisposable
    {
        private readonly BattleFlow _battleFlow;
        private readonly IAdsService _adsService;
        private readonly RemoteConfigService _configService;

        private int _battleCount = 0;

        public BattleAdsTracker(BattleFlow battleFlow, IAdsService adsService, RemoteConfigService configService)
        {
            _battleFlow = battleFlow;
            _adsService = adsService;
            _configService = configService;
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

            if (_configService.Data.AdsConfig.InterstitialInterval > 0 && _battleCount % _configService.Data.AdsConfig.InterstitialInterval == 0)
            {
                _adsService.ShowInterstitialAd();
            }
        }
    }
}