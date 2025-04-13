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
                        dialogue = "scene_opening_blackwood_estate",
                        storyIndex = 0
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

            [JsonProperty(PropertyName = STORY_INDEX)]
            public int storyIndex;
            public const string STORY_INDEX = "b";
        }

        public override string GetSaveDataId()
        {
            return "a";
        }
    }
}
