using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;
using System;

namespace FirstProject.Ads
{
    public class UnityAdsService : IInitializable, IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string ANDROID_GAME_ID = "800361231";
        private const string REWARDED_AD_UNIT = "Rewarded_Android";
        private const string INTERSTITIAL_AD_UNIT = "Interstitial_Android";

        private Action _onRewardCallback;

        public void Initialize()
        {
            Advertisement.Initialize(ANDROID_GAME_ID, true, this);
        }

        public void ShowRewardedAd(Action onRewardEarned)
        {
            _onRewardCallback = onRewardEarned;
            Advertisement.Load(REWARDED_AD_UNIT, this);
        }

        public void ShowInterstitialAd()
        {
            Advertisement.Load(INTERSTITIAL_AD_UNIT, this);
        }

        public void OnUnityAdsAdLoaded(string adUnitID)
        {
            Advertisement.Show(adUnitID, this);
        }

        public void OnUnityAdsFailedToLoad(string adUnitID, UnityAdsLoadError error, string message)
        {
            if (adUnitID == REWARDED_AD_UNIT)
            {
                _onRewardCallback = null;
            }
            Debug.Log($"Ad load failed: {message}");
        }

        public void OnUnityAdsShowComplete(string adUnitID, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitID != REWARDED_AD_UNIT)
            {
                return;
            }

            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                _onRewardCallback?.Invoke();
                Debug.Log("Игрок посмотрел рекламу. Выдача награды");
            }

            _onRewardCallback = null;
        }

        public void OnUnityAdsShowFailure(string adUnitID, UnityAdsShowError error, string message)
        {
            if (adUnitID == REWARDED_AD_UNIT)
            {
                _onRewardCallback = null;
            }
        }

        public void OnUnityAdsShowStart(string adUnitID)
        {

        }

        public void OnUnityAdsShowClick(string adUnitID)
        {

        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads Initialized");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Ads Init Failed: {message}");
        }
    }
}