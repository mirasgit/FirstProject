using Cysharp.Threading.Tasks;
using FirstProject.Core;
using System;
using System.Threading;
using Zenject;

namespace FirstProject.Menu.UI
{
    public class SaveConflictResolver : IInitializable, IDisposable, ISaveConflictResolver
    {
        private readonly SaveConflictView _view;
        private UniTaskCompletionSource<SaveData> _completionSource;

        private SaveData _localData;
        private SaveData _cloudData;

        public SaveConflictResolver(SaveConflictView view)
        {
            _view = view;
        }

        public void Initialize()
        {
            _view.LocalButtonPressed += OnLocalButtonPressed;
            _view.CloudButtonPressed += OnCloudButtonPressed;
            _view.Subscribe();
        }

        public void Dispose()
        {
            _view.LocalButtonPressed -= OnLocalButtonPressed;
            _view.CloudButtonPressed -= OnCloudButtonPressed;
            _view.Unsubscribe();
        }

        public async UniTask<SaveData> ResolveConflictAsync(SaveData localData, SaveData cloudData, CancellationToken token = default)
        {
            _localData = localData;
            _cloudData = cloudData;

            _completionSource = new();

            DateTime localTime = new DateTime(localData.LastSaveTimeTicks, DateTimeKind.Utc).ToLocalTime();
            DateTime cloudTime = new DateTime(cloudData.LastSaveTimeTicks, DateTimeKind.Utc).ToLocalTime();

            string localInfo = $"Local\n Date: {localTime:g}";
            string cloudInfo = $"Cloud\n Date: {cloudTime:g}";

            _view.Show(localInfo, cloudInfo);

            return await _completionSource.Task.AttachExternalCancellation(token);
        }

        private void OnLocalButtonPressed()
        {
            _view.Hide();
            _completionSource.TrySetResult(_localData);
            _completionSource = null;
            _localData = null;
            _cloudData = null;
        }

        private void OnCloudButtonPressed()
        {
            _view.Hide();
            _completionSource.TrySetResult(_cloudData);
            _completionSource = null;
            _localData = null;
            _cloudData = null;
        }
    }
}