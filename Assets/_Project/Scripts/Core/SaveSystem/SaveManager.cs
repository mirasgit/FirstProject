using Cysharp.Threading.Tasks;
using System.Threading;

namespace FirstProject.Core
{
    public class SaveManager : ISaveService
    {
        private readonly LocalSaveProvider _local;
        private readonly CloudSaveProvider _cloud;

        public SaveManager(LocalSaveProvider localSaveProvider, CloudSaveProvider cloudSaveProvider)
        {
            _local = localSaveProvider;
            _cloud = cloudSaveProvider;
        }

        public async UniTask<SaveData> LoadAsync(ISaveConflictResolver resolver, CancellationToken token)
        {
            SaveData localData = await _local.LoadAsync(token);
            SaveData cloudData = await _cloud.LoadAsync(token);

            if (localData == null && cloudData == null)
            {
                return new SaveData();
            }

            if (cloudData == null)
            {
                return localData;
            }

            if (localData == null)
            {
                await _local.SaveAsync(cloudData, token);
                return cloudData;
            }

            if (localData.LastSaveTimeTicks != cloudData.LastSaveTimeTicks)
            {
                SaveData resolvedData = await resolver.ResolveConflictAsync(localData, cloudData, token);

                await SaveAsync(resolvedData, token);
                return resolvedData;
            }

            return cloudData;
        }

        public async UniTask SaveAsync(SaveData data, CancellationToken token)
        {
            await _local.SaveAsync(data, token);
            await _cloud.SaveAsync(data, token);
        }

    }
}
