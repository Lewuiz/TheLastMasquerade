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
                {PLAYER_NAME, "TESTING"},
            };
        }

        public const string PLAYER_NAME = "player_name";

        public override string GetSaveDataId()
        {
            return "a";
        }
    }
}
