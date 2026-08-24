using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FirstProject.Core
{
    public interface IResourceProvider
    {
        UniTask<T> LoadAssetAsync<T>(string address) where T : Object;
        void ReleaseAsset(Object asset);
    }
}