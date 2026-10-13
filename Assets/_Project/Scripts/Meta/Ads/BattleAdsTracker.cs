using FirstProject.Core.Battle;
using FirstProject.Meta.Configs;
using FirstProject.Meta.Multiplayer;
using FirstProject.Meta.Shop;
using System;
using Zenject;

namespace FirstProject.Meta.Ads
{
    public class BattleAdsTracker : IInitializable, IDisposable
    {
        private readonly BattleFlow _battleFlow;
        private readonly IAdsService _adsService;
        private readonly IRemoteConfigService _configService;
        private readonly ProgressModel _progressModel;
        private readonly IMultiplayerService _multiplayerService;

        private int _battleCount = 0;

        public BattleAdsTracker(BattleFlow battleFlow,
            IAdsService adsService,
            IRemoteConfigService configService,
            ProgressModel model, IMultiplayerService multiplayerService)
        {
            _battleFlow = battleFlow;
            _adsService = adsService;
            _configService = configService;
            _progressModel = model;
            _multiplayerService = multiplayerService;
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
            if (_multiplayerService.InRoom)
            {
                return;
            }

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