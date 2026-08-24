using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace FirstProject.Core
{
    public class AddressablesProvider : IResourceProvider
    {
        public async UniTask<T> LoadAssetAsync<T>(string address) where T: Object
        {
            var handle = Addressables.LoadAssetAsync<T>(address);

            return await handle.ToUniTask();
        }

        public void ReleaseAsset(Object asset)
        {
            Addressables.Release(asset);
        }
    }
}