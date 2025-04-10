using System;
using System.Collections.Generic;

namespace Main
{
    public class SaveManager
    {
        private Dictionary<Type, Dictionary<string, object>> dataCache = new();

        public SaveManager()
        {
            Load<PlayerData>();
        }

        public void Load<T>() where T : SaveData, new()
        {
            var instance = new T();
            var dataId = instance.GetSaveDataId();
            var loaded = SaveSystem.LoadGame(dataId) ?? new Dictionary<string, object>();
            var defaults = instance.GetDefaultValue();

            // Merge default + loaded
            foreach (var pair in defaults)
                if (!loaded.ContainsKey(pair.Key))
                    loaded[pair.Key] = pair.Value;

            dataCache[typeof(T)] = loaded;
        }

        public object Get<T>(string key) where T : SaveData, new()
        {
            if (!dataCache.TryGetValue(typeof(T), out var dict))
                throw new Exception($"[SaveManager] Type {typeof(T).Name} not loaded");

            if (!dict.TryGetValue(key, out var val))
                throw new Exception($"[SaveManager] Key '{key}' not found in type '{typeof(T).Name}'");

            return val;
        }

        public void Set<T>(string key, object value) where T : SaveData, new()
        {
            if (!dataCache.ContainsKey(typeof(T)))
                dataCache[typeof(T)] = new Dictionary<string, object>();

            dataCache[typeof(T)][key] = value;
            Save<T>();
        }

        private void Save<T>() where T : SaveData, new()
        {
            var instance = new T();
            var dataId = instance.GetSaveDataId();

            if (dataCache.TryGetValue(typeof(T), out var dict))
                SaveSystem.SaveGame(dataId, dict);
        }
    }
}
