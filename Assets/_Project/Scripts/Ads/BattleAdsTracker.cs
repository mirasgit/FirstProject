using FirstProject.Battle;
using FirstProject.Configs;
using FirstProject.Shop;
using System;
using Zenject;

namespace FirstProject.Ads
{
    public class BattleAdsTracker : IInitializable, IDisposable
    {
        private readonly BattleFlow _battleFlow;
        private readonly IAdsService _adsService;
        private readonly IRemoteConfigService _configService;
        private readonly ProgressModel _progressModel;

        private int _battleCount = 0;

        public BattleAdsTracker(BattleFlow battleFlow, IAdsService adsService, IRemoteConfigService configService, ProgressModel model)
        {
            _battleFlow = battleFlow;
            _adsService = adsService;
            _configService = configService;
            _progressModel = model;
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
            if (!_progressModel.IsAdsRemoved)
            {
                if (_configService.Data.AdsConfig.InterstitialInterval > 0 && _battleCount % _configService.Data.AdsConfig.InterstitialInterval == 0)
                {
                    _adsService.ShowInterstitialAd();
                }
            }
        }
    }
}