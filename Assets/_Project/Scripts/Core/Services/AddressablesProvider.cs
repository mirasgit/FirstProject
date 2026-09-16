using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FirstProject.Core
{
    public class AddressablesProvider : IResourceProvider
    {
        private readonly Dictionary<Object, AsyncOperationHandle> _handles = new();

        public async UniTask<T> LoadAssetAsync<T>(string address, CancellationToken token = default) where T : Object
        {
            var handle = Addressables.LoadAssetAsync<T>(address);

            try
            {
                T asset = await handle.ToUniTask(cancellationToken: token);
                _handles[asset] = handle;
                return asset;
            }
            catch
            {
                Addressables.Release(handle);
                throw;
            }
        }

        public void ReleaseAsset(Object asset)
        {
            if (asset == null) return;

            if (_handles.TryGetValue(asset, out var handle))
            {
                Addressables.Release(handle);
                _handles.Remove(asset);
            }
        }

        public void ReleaseAll()
        {
            foreach (var handle in _handles.Values)
            {
                Addressables.Release(handle);
            }
            _handles.Clear();
        }

        public async UniTask DownloadDependenciesAsync(IEnumerable<string> keys, CancellationToken token = default)
        {
            var handle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
            try
            {
                await handle.ToUniTask(cancellationToken: token);

                Addressables.Release(handle);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Bundle download failed: {ex.Message}");
                throw;
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        public void ClearCache(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                Addressables.ClearDependencyCacheAsync(key);
            }
            Debug.Log("Addressables cache cleared!");
        }
    }
}