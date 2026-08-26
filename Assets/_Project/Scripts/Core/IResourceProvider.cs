using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace FirstProject.Core
{
    public interface IResourceProvider
    {
        UniTask<T> LoadAssetAsync<T>(string address, CancellationToken token = default) where T : Object;
        void ReleaseAsset(Object asset);
    }
}