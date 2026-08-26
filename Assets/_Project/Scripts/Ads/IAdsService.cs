using System;

namespace FirstProject.Ads
{
    public interface IAdsService
    {
        void ShowRewardedAd(Action onRewardEarned);
        void ShowInterstitialAd();
    }
}