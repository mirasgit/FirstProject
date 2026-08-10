using UnityEngine;

namespace FirstProject.Shop
{
    public class SaveService : ISaveService
    {
        private const string SAVE_KEY = "MyGameSave";

        public SaveData Load()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_KEY));
            }
            return new SaveData();
        }

        public void Save(SaveData data)
        {
            PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }
    }   
}