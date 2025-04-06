using System;
using UnityEngine;

namespace Main
{
    public static class SaveDataExtension
    {
        public static T ToObject<T>(this object obj) where T : class
        {
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }

            string json = JsonUtility.ToJson(obj);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
