using Newtonsoft.Json;
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

        private void Load<T>() where T : SaveData, new()
        {
            var instance = new T();
            var dataId = instance.GetSaveDataId();
            var loaded = SaveSystem.LoadGame(dataId) ?? new Dictionary<string, object>();
            var defaults = instance.GetDefaultValue();
            CombineDefaultValue(loaded, defaults);
            dataCache[typeof(T)] = loaded;
        }

        private void CombineDefaultValue(Dictionary<string, object> loaded, Dictionary<string, object> defaults)
        {
            foreach (var pair in defaults)
            {
                if (!loaded.ContainsKey(pair.Key))
                {
                    loaded[pair.Key] = pair.Value;
                }
                else
                {
                    var defaultObj = pair.Value;
                    var savedObj = loaded[pair.Key];

                    if (defaultObj != null && defaultObj.GetType().IsClass && !(defaultObj is string))
                    {
                        string defaultJson = JsonConvert.SerializeObject(defaultObj);
                        string savedJson = JsonConvert.SerializeObject(savedObj);

                        var defaultInstance = JsonConvert.DeserializeObject(defaultJson, defaultObj.GetType());
                        var savedInstance = JsonConvert.DeserializeObject(savedJson, defaultObj.GetType());

                        foreach (var field in defaultObj.GetType().GetFields())
                        {
                            var val = field.GetValue(savedInstance);
                            var defVal = field.GetValue(defaultInstance);

                            if (val == null && defVal != null)
                                field.SetValue(savedInstance, defVal);
                        }

                        loaded[pair.Key] = savedInstance;
                    }
                }
            }
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
