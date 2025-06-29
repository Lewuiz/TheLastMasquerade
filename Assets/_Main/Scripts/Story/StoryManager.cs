using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class StoryManager : MonoBehaviour
    {
        [SerializeField] private ChapterDatabase chapterDatabase = default;

        public ChapterDatabase ChapterDatabase => chapterDatabase;

        private SaveManager saveManager = default;
        private PlayerData.StoryProgress storyProgress = default;

        public string CurrentDialogueId => storyProgress.dialogue;
        public int CurrentChapter => storyProgress.chapter;

        public void Init()
        {
            saveManager = GameCore.Instance.SaveManager;
            storyProgress = saveManager.Get<PlayerData>(PlayerData.STORY_PROGRESS).ToObject<PlayerData.StoryProgress>();

            CheckDialogueSaveData();
        }

        private void CheckDialogueSaveData()
        {
            bool isNewPlayer = string.IsNullOrWhiteSpace(storyProgress.dialogue) || string.IsNullOrEmpty(storyProgress.dialogue);
            if (!isNewPlayer)
                return;

            storyProgress.dialogue = GetChapterData(0).dialogueList[0].dialogueId;
            Save();
        }

        public ChapterDialogue GetDialogue(string dialogueId)
        {
            for(int i = 0; i < chapterDatabase.chapters.Count; i++)
            {
                ChapterDialogue chapterData = chapterDatabase.chapters[i].dialogueList.Find(dialogue => dialogue.dialogueId == dialogueId);
                if (chapterData != null)
                    return chapterData;
            }
            return null;
        }

        public ChapterDialogue GetDialogue(ChapterData chapterData, string dialogueId)
        {
            ChapterDialogue chapterDialogue = chapterData.dialogueList.Find(dialogue => dialogue.dialogueId == dialogueId);
            return chapterDialogue;
        }

        public ChapterData GetChapterData(int chapter)
        {
            return chapterDatabase.chapters[chapter];
        }

        public void UpdateChapter(int chapter)
        {
            storyProgress.chapter = chapter;
            Save();
        }

        public void UpdateDialogueId(string progressId)
        {
            storyProgress.dialogue = progressId;
            Save();
        }

        public void CompleteMiniGame(string miniGameId)
        {
            if (string.IsNullOrEmpty(miniGameId) || string.IsNullOrWhiteSpace(miniGameId))
                return;

            storyProgress.miniGamePlayedList.Add(miniGameId);
            Save();
        }

        public bool HasMiniGamePlayed(string miniGameId)
        {
            return storyProgress.miniGamePlayedList.Contains(miniGameId);
        }

        private void Save()
        {
            saveManager.Set<PlayerData>(PlayerData.STORY_PROGRESS, storyProgress);
        }
    }
}
