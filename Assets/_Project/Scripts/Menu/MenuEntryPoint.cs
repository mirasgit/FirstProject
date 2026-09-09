using Zenject;
using UnityEngine;
using FirstProject.Configs;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using FirstProject.Authentication;
using FirstProject.Shop;
using FirstProject.Core;

namespace FirstProject.Menu
{
    public class MenuEntryPoint : IInitializable, IDisposable
    {
        private readonly IRemoteConfigService _configService;
        private readonly CancellationTokenSource _cts = new();
        private readonly UGSAuthService _authService;
        private readonly ProgressModel _progressModel;
        private readonly ISaveConflictResolver _conflictResolver;
        
        public MenuEntryPoint(IRemoteConfigService remoteConfigService, UGSAuthService authService, ProgressModel progress, ISaveConflictResolver resolver)
        {
            _configService = remoteConfigService;
            _authService = authService;
            _progressModel = progress;
            _conflictResolver = resolver;
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
                await _authService.LoginAnonymouslyAsync(_cts.Token);

                await _configService.FetchConfigAsync(_cts.Token);

                await _progressModel.InitializeDataAsync(_conflictResolver, _cts.Token);

                Debug.Log("Menu is ready to work");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}