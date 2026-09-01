using System;
using UnityEngine;
using Firebase.RemoteConfig;
using Cysharp.Threading.Tasks;
using FirstProject.MatchupConfigs;
using Firebase;

namespace FirstProject.Configs
{
    public class RemoteConfigService 
    {
        private const string CONFIG_KEY = "game_config";

        public event Action OnConfigLoaded;

        public GameConfigData Data { get; private set; }

        public async UniTask FetchConfigAsync()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;

            await remoteConfig.FetchAsync(TimeSpan.Zero).AsUniTask();
            await remoteConfig.ActivateAsync().AsUniTask();

            string json = remoteConfig.GetValue(CONFIG_KEY).StringValue;

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("?? Remote Config JSON is empty or key not found!");
                throw new InvalidOperationException("Remote Config game_config is empty or missing.");
            }

            Data = JsonUtility.FromJson<GameConfigData>(json);
            Debug.Log("? Remote Config successfully loaded and parsed!");

            OnConfigLoaded?.Invoke();
        }

        public CharacterSettings GetCharacterConfig(CharacterClass characterClass)
        {
            foreach (var config in Data.Characters)
            {
                if (config.ClassType == characterClass)
                {
                    return config;
                }
            }

            throw new InvalidOperationException(
                $"Character config not found: {characterClass}");
        }
    }
}