using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class StoryRunner : MonoBehaviour
    {
        private StoryManager storyManager = default;

        private StoryData storyData = default;
        private int dialogueDataIdx = -1;

        private List<DialogueCharacterData> dialogueCharacterDataList = new List<DialogueCharacterData>();
        private int dialogueCharacterIdx = -1;

        private Action<DialogueCharacterData> updateDialoguePanel = default;
        private Action<string> updateActorConversation = default;
        private Action<DialogueActorControl> checkCharacterControl = default;
        private Action<List<string>> addCharacter = default;
        private Func<bool> isAnimating = default;
        private Action<List<DialogueEventData>> executeDialogueEvent = default;
        private Action backToChapterSelectionScene = default;

        public bool IsChapterEnded { get; private set; } = false;

        public void Init(StoryRunnerData data)
        {
            storyManager = data.storyManager;
            updateDialoguePanel = data.updateDialoguePanel;
            updateActorConversation = data.updateActorConversation;
            checkCharacterControl = data.checkCharacterControl;
            addCharacter = data.addCharacter;
            isAnimating = data.isAnimating;
            executeDialogueEvent = data.executeDialogueEvent;
            backToChapterSelectionScene = data.backToChapterSelectionScene;

            IsChapterEnded = false;

            storyData = storyManager.GetChapterData(storyManager.CurrentChapter);
            dialogueDataIdx = storyData.dialogueDataList.FindIndex(dialogue => dialogue.dialogueId == storyManager.CurrentDialogueId);
            dialogueCharacterDataList = storyData.dialogueDataList[dialogueDataIdx].dialogue;

            AddCharacterByDefault();
            PlayNextDialogue();
        }

        private void AddCharacterByDefault()
        {
            var dialogue = storyData.dialogueDataList[dialogueDataIdx];
            addCharacter?.Invoke(dialogue.defaultActors);
        }

        public void PlayNextDialogue()
        {
            StartCoroutine(PlayNextDialogueCor());
        }

        private IEnumerator PlayNextDialogueCor()
        {
            while(isAnimating.Invoke())
                yield return null;

            dialogueCharacterIdx++;
            bool isCharacterDialogueEnded = dialogueCharacterIdx >= dialogueCharacterDataList.Count;

            if (isCharacterDialogueEnded)
            {
                OnDialogueEnded();
            }

            DialogueCharacterData dialogueCharacterData = dialogueCharacterDataList[dialogueCharacterIdx];
            ExecuteDialogEvent(dialogueCharacterData);

            checkCharacterControl?.Invoke(dialogueCharacterData.actorControl);
            updateDialoguePanel?.Invoke(dialogueCharacterData);
            updateActorConversation?.Invoke(dialogueCharacterData.character);
        }

        private void ExecuteDialogEvent(DialogueCharacterData dialogueCharacterData)
        {
            var dialogueEvents = dialogueCharacterData.events;
            executeDialogueEvent?.Invoke(dialogueEvents);
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
                AddCharacterByDefault();
            }
        }

        private void OnChapterEnded()
        {
            StartCoroutine(OnChapterEndedCor());
        }

        private IEnumerator OnChapterEndedCor()
        {
            IsChapterEnded = true;

            int nextChapter = storyManager.CurrentChapter + 1;
            var nextChapterData = storyManager.GetChapterData(nextChapter);

            if (nextChapterData == null)
            {
                //storyManager.UpdateStoryProgress("completed", nextChapter);
                //return to main menu
            }
            else
            {
                string nextDialogueId = storyData.dialogueDataList[dialogueDataIdx].nextDialogueId;
                storyManager.UpdateStoryProgress(nextDialogueId, nextChapter);
            }

            while (isAnimating.Invoke())
                yield return null;

            backToChapterSelectionScene?.Invoke();
        }
    }
}
