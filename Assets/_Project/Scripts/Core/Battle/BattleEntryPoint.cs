using Cysharp.Threading.Tasks;
using FirstProject.Meta.Configs;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace FirstProject.Core.Battle
{
    public class BattleEntryPoint : IInitializable, IDisposable
    {
        private readonly BattleFlow _battleFlow;
        private readonly IRemoteConfigService _configService;
        private readonly CancellationTokenSource _cts = new();

        public BattleEntryPoint(BattleFlow battleflow, IRemoteConfigService configService)
        {
            _battleFlow = battleflow;
            _configService = configService;
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }

        private async UniTaskVoid InitializeAsync()
        {
            try
            {
                await _configService.FetchConfigAsync(_cts.Token);

                _battleFlow.ShowStartScreen();
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}