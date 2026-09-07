using Zenject;
using UnityEngine;
using FirstProject.Configs;
using Cysharp.Threading.Tasks;
using System;

namespace FirstProject.Menu
{
    public class MenuEntryPoint : IInitializable
    {
        private readonly IRemoteConfigService _configService;
        
        public MenuEntryPoint(IRemoteConfigService remoteConfigService)
        {
            _configService = remoteConfigService;
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }

        private async UniTaskVoid InitializeAsync()
        {
            try
            {
                await _configService.FetchConfigAsync();
                Debug.Log("Menu is ready to work");
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
            }
        }
    }
}