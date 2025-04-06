using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Main
{
    [Serializable]
    public class PlayerData : SaveData
    {
        public override Dictionary<string, object> GetDefaultValue()
        {
            return new Dictionary<string, object>()
            {
                {name, "TESTING"}
            };
        }

        [JsonProperty(PropertyName = NAME)]
        public string name = default;
        public const string NAME = "A";

        public override string GetSaveDataId()
        {
            return "a";
        }
    }
}
