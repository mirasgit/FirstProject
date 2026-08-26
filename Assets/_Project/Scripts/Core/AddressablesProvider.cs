using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System.Threading;

namespace FirstProject.Core
{
    public class AddressablesProvider : IResourceProvider
    {
        public async UniTask<T> LoadAssetAsync<T>(string address, CancellationToken token = default) where T: Object
        {
            var handle = Addressables.LoadAssetAsync<T>(address);

            try
            {
                return await handle.ToUniTask(cancellationToken: token);
            }
            catch
            {
                Addressables.Release(handle);
                throw;
            }
            
        }

        public void ReleaseAsset(Object asset)
        {
            Addressables.Release(asset);
        }
    }
}