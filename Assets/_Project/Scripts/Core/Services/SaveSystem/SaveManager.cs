using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace FirstProject.Core.SaveSystem
{
    public class SaveManager : ISaveService
    {
        private readonly IEnumerable<ISaveProvider> _providers;

        public SaveManager(IEnumerable<ISaveProvider> providers)
        {
            _providers = providers;
        }

        public async UniTask<SaveData> LoadAsync(CancellationToken token)
        {
            long lastSaveTimeTicks = 0;
            var loadData = from p in _providers select p.LoadAsync(token);
            var newestData = await UniTask.WhenAll(loadData);
            SaveData latestSave = null;

            foreach (var data in newestData)
            {
                if (data == null)
                {
                    continue;
                }

                if (data.LastSaveTimeTicks > lastSaveTimeTicks)
                {
                    lastSaveTimeTicks = data.LastSaveTimeTicks;
                    latestSave = data;
                }
            }

            if (latestSave == null)
            {
                return new SaveData();
            }

            SaveAsync(latestSave, token).Forget();
            return latestSave;
        }   

        public async UniTask SaveAsync(SaveData data, CancellationToken token)
        {
            var saveData = from p in _providers select p.SaveAsync(data, token);
            await UniTask.WhenAll(saveData);
        }
    }
}
