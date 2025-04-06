using System;
using System.Collections.Generic;

namespace Main
{
    public class SaveManager
    {
        private Dictionary<System.Type, SaveData> dataCache = new();

        public SaveManager()
        {
            LoadAllData();
        }

        private void LoadAllData()
        {
            LoadAndCache<PlayerData>();
        }

        private void LoadAndCache<T>() where T : SaveData, new()
        {
            var data = SaveSystem.LoadGame<T>();
            dataCache[typeof(T)] = data;
        }

        public object Get<T>(string key) where T : SaveData, new()
        {
            if (!dataCache.ContainsKey(typeof(T)))
                throw new System.Exception($"[{nameof(SaveManager)}] Data of type '{typeof(T).Name}' is not cached. Make sure it is loaded.");

            var data = dataCache[typeof(T)];

            var field = typeof(T).GetField(key);
            if (field == null)
                throw new Exception($"[{nameof(SaveManager)}] field '{key}' not found in type '{typeof(T).Name}'.");

            return field.GetValue(data);
        }

        public void Set<T>(T newData) where T : SaveData
        {
            dataCache[typeof(T)] = newData;
            SaveSystem.SaveGame(newData.GetSaveDataId(), newData);
        }
    }
}
