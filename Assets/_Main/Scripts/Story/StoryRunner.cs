using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Main
{
    public class StoryRunner : MonoBehaviour
    {
        private StoryManager storyManager = default;

        private StoryData storyData = default;
        private int dialogueDataIdx = -1;

        private List<DialogueCharacterData> dialogueCharacterDataList = new List<DialogueCharacterData>();
        private int dialogueCharacterIdx = 0;
        private DialogueCharacterData CurrentCharacterDialogue => dialogueCharacterDataList[dialogueCharacterIdx];

        private Action<DialogueCharacterData> updateDialoguePanel = default;

        public void Init(StoryManager storyManager, Action<DialogueCharacterData> updateDialoguePanel)
        {
            this.storyManager = storyManager;
            this.updateDialoguePanel = updateDialoguePanel;

            storyData = storyManager.GetChapterData(storyManager.CurrentChapter);
            dialogueDataIdx = storyData.dialogueDataList.FindIndex(dialogue => dialogue.dialogueId == storyManager.CurrentDialogueId);
            dialogueCharacterDataList = storyData.dialogueDataList[dialogueDataIdx].dialogue;

            updateDialoguePanel?.Invoke(CurrentCharacterDialogue);
        }

        public void PlayNextDialogue()
        {
            dialogueCharacterIdx++;
            bool isCharacterDialogueEnded = dialogueCharacterIdx >= dialogueCharacterDataList.Count;

            if (isCharacterDialogueEnded)
            {
                OnDialogueEnded();
            }

            updateDialoguePanel?.Invoke(CurrentCharacterDialogue);
        }

        private void OnDialogueEnded()
        {
            dialogueCharacterIdx = 0;
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
                dialogueCharacterDataList = storyData.dialogueDataList[dialogueDataIdx].dialogue;
            }
        }

        private void OnChapterEnded()
        {
            int nextChapter = storyManager.CurrentChapter + 1;
            var nextChapterData = storyManager.GetChapterData(nextChapter);

            if (nextChapterData == null)
            {
                //storyManager.UpdateStoryProgress("completed", nextChapter);
            }
            else
            {
                string nextDialogueId = storyData.dialogueDataList[dialogueDataIdx].nextDialogueId;
                storyManager.UpdateStoryProgress(nextDialogueId, nextChapter);
            }
            //will load to level selection maybe?
            Debug.Log("On Chapter Ended");
        }
    }
}
