using System;
using UnityEngine;

namespace FirstProject.Ads
{
    public interface IAdsService
    {
        void ShowRewardedAd(Action onRewardEarned);
        void ShowInterstitialAd();
    }
}