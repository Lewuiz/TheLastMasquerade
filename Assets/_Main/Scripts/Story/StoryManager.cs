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

        public string CurrentDialogueId => storyProgress.dialogue;
        public int CurrentChapter => storyProgress.chapter;

        private List<StoryData> storyDataList = new List<StoryData>();

        public void Init()
        {
            saveManager = GameCore.Instance.SaveManager;
            storyProgress = saveManager.Get<PlayerData>(PlayerData.STORY_PROGRESS).ToObject<PlayerData.StoryProgress>();

            for (int i = 0; i < dialogueDataList.Count; i++)
            {
                List<DialogueData> dialogueDataList = JsonConvert.DeserializeObject<List<DialogueData>>(this.dialogueDataList[i].text);
                StoryData storyData = new StoryData()
                {
                    chapter = i,
                    dialogueDataList = dialogueDataList
                };
                storyDataList.Add(storyData);
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

        public StoryData GetChapterData(int chapter)
        {
            return storyDataList.Find(storyData => storyData.chapter == chapter);
        }

        public void UpdateStoryProgress(string progressId, int chapter)
        {
            storyProgress.dialogue = progressId;
            storyProgress.chapter = chapter;

            Save();
        }

        public int GetTotalActorOnEachDialogue()
        {

            return 0;
        }

        private void Save()
        {
            saveManager.Set<PlayerData>(PlayerData.STORY_PROGRESS, storyProgress);
        }
    }
}
