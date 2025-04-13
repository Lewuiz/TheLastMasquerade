using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Main
{
    public static class SaveSystem
    {
        private const string KEY = "b14ca5898a4e4133bbce2ea2315a1916"; //this key need to have 32 long character because we are using AES-256

        public static void SaveGame(string id, Dictionary<string, object> data)
        {
            string json = JsonConvert.SerializeObject(data);
            string encrypted = AesOperation.EncryptString(KEY, json);

            string path = Path.Combine(Application.persistentDataPath, $"{id}.sav");
            File.WriteAllText(path, encrypted);
        }

        public static Dictionary<string, object> LoadGame(string id)
        {
            string path = Path.Combine(Application.persistentDataPath, $"{id}.sav");
            if (!File.Exists(path)) 
                return null;

            string encrypted = File.ReadAllText(path);
            string decrypted = AesOperation.DecryptString(KEY, encrypted);
            Debug.Log(decrypted);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(decrypted);
        }
    }
}
