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
                {
                    STORY_PROGRESS, new StoryProgress()
                    {
                        dialogue = "scene_1a",
                        chapter = 0,
                        puzzlePlayedList = new List<string>()
                    }
                },
                {
                    INVENTORY, new Inventory()
                    {
                        inspectItemList = new List<string>(),   
                    }
                }
            };
        }

        public const string STORY_PROGRESS = "story_progress";

        [Serializable]
        public class StoryProgress
        {
            [JsonProperty(PropertyName = DIALOGUE)]
            public string dialogue;
            public const string DIALOGUE = "a";

            [JsonProperty(PropertyName = CHAPTER)]
            public int chapter;
            public const string CHAPTER = "b";

            [JsonProperty(PropertyName = PUZZLE_PLAYED_LIST)]
            public List<string> puzzlePlayedList;
            public const string PUZZLE_PLAYED_LIST = "c";
        }

        public const string INVENTORY = "inventory";

        [Serializable]
        public class Inventory
        {
            [JsonProperty(PropertyName = INSPECT_ITEM_LIST)]
            public List<string> inspectItemList;
            public const string INSPECT_ITEM_LIST = "a";
        }

        public override string GetSaveDataId()
        {
            return "a";
        }
    }
}
