using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using FirstProject.Core.SaveSystem;
using FirstProject.Core;
using FirstProject.Meta.Configs;
using FirstProject.Meta.Shop;
using FirstProject.Meta.Authentication;

namespace FirstProject.Meta.Menu
{
    public class MenuEntryPoint : IInitializable, IDisposable
    {
        private readonly IRemoteConfigService _configService;
        private readonly CancellationTokenSource _cts = new();
        private readonly UGSAuthService _authService;
        private readonly ProgressModel _progressModel;
        private readonly ISaveConflictResolver _conflictResolver;
        private readonly IResourceProvider _resourceProvider;

        private readonly string[] _remoteKeys = new[] { "Warrior", "Archer", "Wizard" };
        
        public MenuEntryPoint(
            IRemoteConfigService remoteConfigService,
            UGSAuthService authService,
            ProgressModel progress,
            ISaveConflictResolver resolver,
            IResourceProvider resourceProvider)
        {
            _configService = remoteConfigService;
            _authService = authService;
            _progressModel = progress;
            _conflictResolver = resolver;
            _resourceProvider = resourceProvider;
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
                _resourceProvider.ClearCache(_remoteKeys);

                await _authService.LoginAnonymouslyAsync(_cts.Token);

                await _configService.FetchConfigAsync(_cts.Token);

                Debug.Log("Starting to download remote bundles");
                await _resourceProvider.DownloadDependenciesAsync(_remoteKeys, _cts.Token);
                Debug.Log("Bundles downloaded successfully");

                await _progressModel.InitializeDataAsync(_conflictResolver, _cts.Token);

                Debug.Log("Menu is ready to work");
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