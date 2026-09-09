using System;
using UnityEngine;
using Firebase.RemoteConfig;
using Cysharp.Threading.Tasks;
using Firebase;
using Newtonsoft.Json;
using System.Threading;
using FirstProject.MatchupConfigs;

namespace FirstProject.Configs
{
    public class RemoteConfigService : IRemoteConfigService
    {
        private const string CONFIG_KEY = "game_config";

        public GameConfigData Data { get; private set; }

        public async UniTask FetchConfigAsync(CancellationToken token)
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask().AttachExternalCancellation(token);

            if (dependencyStatus != DependencyStatus.Available)
            {
                throw new Exception($"Firebase is not ready. Status: {dependencyStatus}");
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;

            await remoteConfig.FetchAsync(TimeSpan.Zero).AsUniTask().AttachExternalCancellation(token);
            await remoteConfig.ActivateAsync().AsUniTask().AttachExternalCancellation(token);

            string json = remoteConfig.GetValue(CONFIG_KEY).StringValue;

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Remote Config JSON is empty or key not found!");
                throw new InvalidOperationException("Remote Config game_config is empty or missing.");
            }

            Data = JsonConvert.DeserializeObject<GameConfigData>(json);
            Debug.Log("Remote Config successfully loaded and parsed!");
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

            throw new InvalidOperationException($"Character config not found: {characterClass}");
        }
    }
}