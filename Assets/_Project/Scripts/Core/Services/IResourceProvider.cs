using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace FirstProject.Core
{
    public interface IResourceProvider
    {
        UniTask<T> LoadAssetAsync<T>(string address, CancellationToken token = default) where T : Object;
        void ReleaseAsset(Object asset);

        void ReleaseAll();

        UniTask DownloadDependenciesAsync(IEnumerable<string> keys, CancellationToken token = default);
        void ClearCache(IEnumerable<string> keys);
    }
}