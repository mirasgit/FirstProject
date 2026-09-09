using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine;

namespace FirstProject.Core
{
    public class LocalSaveProvider
    {
        private const string SAVE_KEY = "MyGameSave";

        public UniTask<SaveData> LoadAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                var data = JsonConvert.DeserializeObject<SaveData>(PlayerPrefs.GetString(SAVE_KEY));

                return UniTask.FromResult(data);
            }
            return UniTask.FromResult<SaveData>(null);
        }

        public UniTask SaveAsync(SaveData data, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            PlayerPrefs.SetString(SAVE_KEY, JsonConvert.SerializeObject(data));
            PlayerPrefs.Save();
            return UniTask.CompletedTask;
        }
    }   
}