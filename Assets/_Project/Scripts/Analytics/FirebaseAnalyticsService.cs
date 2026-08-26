using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
using Zenject;

namespace FirstProject.Analytics
{
    public class FirebaseAnalyticsService : IInitializable, IAnalyticsService
    {
        private bool _isInitialized;
        public void Initialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Firebase initiation failed");
                    return;
                }

                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    _isInitialized = true;

                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    Debug.Log("Firebaase analytics successfully initialized!"); 
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }
        public void LogEvent(string eventName)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning($"Trying to log event {eventName}, but Firebase is not ready yet.");
                return;
            }

            FirebaseAnalytics.LogEvent(eventName);
            Debug.Log($"Analytics Event logged: {eventName}");
        }
    }
}
