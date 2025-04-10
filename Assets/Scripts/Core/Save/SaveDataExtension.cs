using Newtonsoft.Json;
using System;

namespace Main
{
    public static class SaveDataExtension
    {
        public static T ToObject<T>(this object obj)
        {
            if (obj == null)
                return default;

            Type targetType = typeof(T);

            if (targetType.IsPrimitive || targetType == typeof(string) || targetType.IsValueType)
                return (T)Convert.ChangeType(obj, targetType);

            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
