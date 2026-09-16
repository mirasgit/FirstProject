using Cysharp.Threading.Tasks;
using System.Threading;

namespace FirstProject.Core.SaveSystem
{
    public class SaveManager : ISaveService
    {
        private readonly ISaveProvider _local;
        private readonly ISaveProvider _cloud;

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

            if (localData != null && cloudData != null && localData.LastSaveTimeTicks != cloudData.LastSaveTimeTicks)
            {
                SaveData resolvedData = await resolver.ResolveConflictAsync(localData, cloudData, token);

                await SaveAsync(resolvedData, token);
                return resolvedData;
            }

            SaveData finalData = cloudData ?? localData;

            return finalData;
        }

        public async UniTask SaveAsync(SaveData data, CancellationToken token)
        {
            await UniTask.WhenAll(_local.SaveAsync(data, token), _cloud.SaveAsync(data, token));
        }
    }
}
