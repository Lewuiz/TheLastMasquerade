using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class StoryManager : MonoBehaviour
    {
        [SerializeField] private List<TextAsset> dialogueDataList = new List<TextAsset>();

        private SaveManager saveManager = default;
        private PlayerData.StoryProgress storyProgress = default;

        private List<StoryData> storyDataList = new List<StoryData>();

        public void Init()
        {
            saveManager = GameCore.Instance.SaveManager;
            storyProgress = saveManager.Get<PlayerData>(PlayerData.STORY_PROGRESS).ToObject<PlayerData.StoryProgress>();

            for (int i = 0; i < dialogueDataList.Count; i++)
            {
                List<StoryData> storyData = JsonConvert.DeserializeObject<List<StoryData>>(dialogueDataList[i].text);
                storyDataList.AddRange(storyData);
            }
        }

        public StoryData GetStoryData(string dialogueId)
        {
            for(int i = 0; i < storyDataList.Count; i++)
            {
                StoryData storyData = storyDataList[i];
                DialogueData dialogueData = storyData.dialogueDataList.Find(dialogueData => dialogueData.dialogueId == dialogueId);
                if (dialogueData != null)
                    return storyData;
            }

            throw new System.Exception($"[{nameof(StoryManager)}] did not find such id: {dialogueId}");
        }

        public void Save()
        {
            saveManager.Set<PlayerData>(PlayerData.STORY_PROGRESS, storyProgress);
        }
    }
}
