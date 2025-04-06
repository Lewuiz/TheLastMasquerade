using System.IO;
using UnityEngine;

namespace Main
{
    public static class SaveSystem
    {
        private const string KEY = "b14ca5898a4e4133bbce2ea2315a1916";

        public static void SaveGame<T>(string saveName, T serializableObject)
        {
            string json = JsonUtility.ToJson(serializableObject);
            string encryptedJson = AesOperation.EncryptString(KEY, json);

#if UNITY_WEBGL
            PlayerPrefs.SetString(saveName, encryptedJson);
            PlayerPrefs.Save();
#else
            string path = Path.Combine(Application.persistentDataPath, $"{saveName}.sav");

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, encryptedJson);

#if UNITY_EDITOR
            Debug.Log($"[SaveSystem] Saved to: {path}");
#endif
#endif
        }

        public static T LoadGame<T>() where T : SaveData, new()
        {
            T instance = new T();
            string saveDataId = instance.GetSaveDataId();
            string path = Path.Combine(Application.persistentDataPath, $"{saveDataId}.sav");

#if UNITY_WEBGL
            if (!PlayerPrefs.HasKey(saveDataId))
            {
                SaveGame(saveDataId, instance);
                return instance;
            }

            string encryptedJson = PlayerPrefs.GetString(saveDataId);
#else
            if (!File.Exists(path))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"[SaveSystem] File not found. Creating default: {path}");
#endif
                SaveGame(saveDataId, instance);
                return instance;
            }

            string encryptedJson = File.ReadAllText(path);
#endif

            string decryptedJson = AesOperation.DecryptString(KEY, encryptedJson);

#if UNITY_EDITOR
            Debug.Log($"[SaveSystem] Decrypted JSON: {decryptedJson}");
#endif

            JsonUtility.FromJsonOverwrite(decryptedJson, instance);
            return instance;
        }
    }
}
