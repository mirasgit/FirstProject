using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FirstProject.Meta.Analytics
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
                    Debug.Log("Firebase analytics successfully initialized!"); 
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }
        public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning($"Trying to log event {eventName}, but Firebase is not ready yet.");
                return;
            }

            if (parameters == null || parameters.Count == 0)
            {
                FirebaseAnalytics.LogEvent(eventName);
            }
            else
            {
                var firebaseParameters = new List<Parameter>();
                foreach (var parameter in parameters)
                {
                    if (parameter.Value is int intValue)
                    {
                        firebaseParameters.Add(new Parameter(parameter.Key, intValue));
                    }
                    else if (parameter.Value is float floatVal)
                    {
                        firebaseParameters.Add(new Parameter(parameter.Key, floatVal));
                    }
                    else
                    {
                        firebaseParameters.Add(new Parameter(parameter.Key, parameter.Value.ToString()));
                    }
                }
                FirebaseAnalytics.LogEvent(eventName, firebaseParameters.ToArray());
            }
            Debug.Log($"Analytics Event logged: {eventName}");
        }
    }
}
