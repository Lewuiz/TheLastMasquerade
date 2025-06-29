using System;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class StoryRunner : MonoBehaviour
    {
        private StoryManager storyManager = default;
        private InventoryManager inventoryManager = default;
        private StoryRunnerData data = default;

        private ChapterData chapterData = default;
        private ChapterDialogue chapterDialogue = default;

        private int conversationIdx = default;

        public bool IsPlaying { get; private set; } = default;
        private bool hasInit = false;
        private bool canProceedDialogue = false;

        private int chapter = 0;

        public event Action OnDialogueEndedEvent = default;
        public event Action OnChapterEndedEvent = default;

        public void Init(StoryRunnerData data)
        {
            inventoryManager = GameCore.Instance.InventoryManager;
            storyManager = data.storyManager;
            this.data = data;
            data.dialoguePanel.SetOnClickEvent(() => canProceedDialogue = true);
            hasInit = true;
        }

        private void SetDefault()
        {
            conversationIdx = 0;
        }

        public void Run()
        {
            SetDefault();

            StorySceneData storySceneData = GameCore.Instance.StorySceneData;

            if (storySceneData.SelectedChapter == -1)
            {
                chapter = storyManager.CurrentChapter;
                chapterData = storyManager.GetChapterData(chapter);
                chapterDialogue = storyManager.GetDialogue(chapterData, storyManager.CurrentDialogueId);
            }
            else
            {
                chapter = storySceneData.SelectedChapter;
                chapterData = storyManager.GetChapterData(chapter);
                chapterDialogue = storyManager.GetDialogue(chapterData, chapterData.dialogueList[0].dialogueId);
            }

            IsPlaying = true;
            canProceedDialogue = true;
        }

        private void Update()
        {
            if (!IsPlaying || !hasInit)
                return;

            if (data.dialoguePanel.IsHiding)
                return;

            if (!canProceedDialogue)
                return;

            CheckDialogue();
        }

        private void CheckDialogue()
        {
            if (chapterDialogue == null)
            {
                OnChapterEnded();
                return;
            }

            if (conversationIdx >= chapterDialogue.dialogueList.Count)
            {
                OnDialogueEnded();
                return;
            }

            StartCoroutine(PlayDialogueCor());

            canProceedDialogue = false;
            conversationIdx++;
        }

        private IEnumerator PlayDialogueCor()
        {
            data.dialoguePanel.SetCanClick(false);

            CharacterDialogue characterDialogue = chapterDialogue.dialogueList[conversationIdx];

            data.actorController.UpdateActorsConversation(characterDialogue);

            while (data.actorController.IsAnimating)
                yield return null;

            string dialogueTitle = characterDialogue.characterData != null ? characterDialogue.characterData.name : "";
            data.dialoguePanel.UpdateDialoguePanel(characterDialogue.text, dialogueTitle);

            data.dialoguePanel.SetCanClick(true);
        }

        private void OnDialogueEnded()
        {
            if (chapterDialogue.dialogueChoiceData.Count > 0)
            {
                data.dialogueChoicePanel.ShowChoiceDialogue(chapterDialogue.dialogueChoiceData, UpdateDialogue);
            }
            else
            {
                UpdateDialogue(chapterDialogue.nextDialogueId);
            }

            SetDefault();
            data.dialoguePanel.SetCanClick(false);
            OnDialogueEndedEvent?.Invoke();
        }

        private void UpdateDialogue(string nextDialogueId)
        {
            if (storyManager.CurrentChapter <= chapter)
                storyManager.UpdateDialogueId(nextDialogueId);
            chapterDialogue = storyManager.GetDialogue(nextDialogueId);
        }

        private void OnChapterEnded()
        {
            storyManager.UpdateChapter(storyManager.CurrentChapter + 1);
            OnChapterEndedEvent?.Invoke();
            data.backToChapterSelectionScene?.Invoke();
            Stop();
        }

        public void Stop()
        {
            IsPlaying = false;
        }
    }
}
