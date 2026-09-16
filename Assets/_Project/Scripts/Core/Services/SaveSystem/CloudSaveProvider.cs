using UnityEngine;
using Unity.Services.CloudSave;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using System;

namespace FirstProject.Core.SaveSystem
{
    public class CloudSaveProvider : ISaveProvider
    {
        private const string CLOUD_KEY = "game_save";

        public async UniTask SaveAsync(SaveData data, CancellationToken token)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data);
                var cloudData = new Dictionary<string, object> { { CLOUD_KEY, json } };
                await CloudSaveService.Instance.Data.Player.SaveAsync(cloudData).AsUniTask().AttachExternalCancellation(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Could not save to cloud: {exception.Message}");
            }
        }

        public async UniTask<SaveData> LoadAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                var savedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { CLOUD_KEY }).AsUniTask().AttachExternalCancellation(token);

                if (savedData.TryGetValue(CLOUD_KEY, out var item))
                {
                    string json = item.Value.GetAs<string>();
                    return JsonConvert.DeserializeObject<SaveData>(json);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Load failed: {exception.Message}");
            }
            return null;
        }
    }
}
