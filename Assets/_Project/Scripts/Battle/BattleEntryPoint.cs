using Cysharp.Threading.Tasks;
using FirstProject.Configs;
using System;
using UnityEngine;
using Zenject;

namespace FirstProject.Battle
{
    public class BattleEntryPoint : IInitializable
    {
        private readonly BattleFlow _battleFlow;
        private readonly RemoteConfigService _configService;

        public BattleEntryPoint(BattleFlow battleflow, RemoteConfigService configService)
        {
            _battleFlow = battleflow;
            _configService = configService;
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

                _battleFlow.ShowStartScreen();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}