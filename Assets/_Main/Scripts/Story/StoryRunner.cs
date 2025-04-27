using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class StoryRunner : MonoBehaviour
    {
        [SerializeField] private DialoguePanel dialoguePanel = default;

        private StoryManager storyManager = default;
        private StoryData storyData = default;
        private int dialogueDataIdx = -1;

        public void Init(StoryManager storyManager, Action CheckDialogueEvent)
        {
            this.storyManager = storyManager;
            storyData = storyManager.GetChapterData(storyManager.CurrentChapter);

            dialoguePanel.Init(OnDialogueEnded, CheckDialogueEvent);

            dialogueDataIdx = storyData.dialogueDataList.FindIndex(dialogue => dialogue.dialogueId == storyManager.CurrentDialogueId);
            PlayDialogue();
        }

        private void PlayDialogue()
        {
            var dialogueData = storyData.dialogueDataList[dialogueDataIdx];
            dialoguePanel.UpdateDialogueData(dialogueData);
            dialoguePanel.UpdateDialoguePanel();
            
        }

        private void OnChapterEnded()
        {
            int nextChapter = storyManager.CurrentChapter + 1;
            storyData = storyManager.GetChapterData(nextChapter);

            bool isStoryCompleted = storyData != null;

            if (isStoryCompleted)
            {
                storyManager.UpdateStoryProgress("completed", nextChapter);
            }
            else
            {
                string dialogueId = storyData.dialogueDataList[0].dialogueId;
                storyManager.UpdateStoryProgress(dialogueId, nextChapter);
            }
        }

        private void OnDialogueEnded()
        {
            dialogueDataIdx++;

            bool isChapterEnded = dialogueDataIdx >= storyData.dialogueDataList.Count;
            if (isChapterEnded)
            {
                OnChapterEnded();
            }
            else
            {
                string dialogueId = storyData.dialogueDataList[dialogueDataIdx].dialogueId;
                storyManager.UpdateStoryProgress(dialogueId, storyManager.CurrentChapter);

                PlayDialogue();
            }
        }
    }
}
