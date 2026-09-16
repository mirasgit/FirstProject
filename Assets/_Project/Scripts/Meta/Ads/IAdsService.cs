using System;

namespace FirstProject.Meta.Ads
{
    public interface IAdsService
    {
        void ShowRewardedAd(Action onRewardEarned);
        void ShowInterstitialAd();
    }
}