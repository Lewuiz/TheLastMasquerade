using Newtonsoft.Json;
using System;

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

            var json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json)!;
        }
    }
}
