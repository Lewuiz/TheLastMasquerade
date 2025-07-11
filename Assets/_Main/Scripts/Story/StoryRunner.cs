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

        private string chapterEndId = default;

        public void Init(StoryRunnerData data)
        {
            inventoryManager = GameCore.Instance.InventoryManager;
            storyManager = data.storyManager;
            this.data = data;
            SetDialoguePanelEvent();
            hasInit = true;
        }

        public void SetDialoguePanelEvent()
        {
            data.dialoguePanel.SetOnClickEvent(ForceProceedDialogue);
        }

        public void ForceProceedDialogue()
        {
            canProceedDialogue = true;
        }

        public void SetDefault()
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

                if (chapterData == null)
                {
                    data.backToChapterSelectionScene?.Invoke();
                    return;
                }

                chapterDialogue = storyManager.GetDialogue(chapterData, storyManager.CurrentDialogueId);
            }
            else
            {
                chapter = storySceneData.SelectedChapter;
                chapterData = storyManager.GetChapterData(chapter);

                if (chapterData == null)
                {
                    data.backToChapterSelectionScene?.Invoke();
                    return;
                }

                chapterDialogue = storyManager.GetDialogue(chapterData, chapterData.dialogueList[0].dialogueId);
            }

            IsPlaying = true;
            canProceedDialogue = true;
        }

        private void Update()
        {
            if (!IsPlaying || !hasInit)
                return;

            if (data.inspectItemController.IsExecuting())
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
            data.storyEventHandler.ExecuteEvents(characterDialogue.chapterDialogueEventList, ChapterDialogueEventPhase.Start);

            data.actorController.UpdateActorsConversation(characterDialogue);

            while (data.actorController.IsAnimating)
                yield return null;

            string dialogueTitle = characterDialogue.characterData != null ? characterDialogue.characterData.name : "Narrator";
            data.dialoguePanel.UpdateDialoguePanel(characterDialogue.text, dialogueTitle);
            data.storyEventHandler.ExecuteEvents(characterDialogue.chapterDialogueEventList, ChapterDialogueEventPhase.End);
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

            if(nextDialogueId =="end_win" || nextDialogueId == "end_lose")
            {
                chapterEndId = nextDialogueId;
            }

            chapterDialogue = storyManager.GetDialogue(nextDialogueId);
        }

        private void OnChapterEnded()
        {
            storyManager.UpdateChapter(storyManager.CurrentChapter + 1);
            OnChapterEndedEvent?.Invoke();
            if (chapterEndId == "end_win" || chapterEndId == "end_lose")
            {
                data.actorController.HideAllActor();
                data.dialoguePanel.Hide();

                GameCompleteWindowData gameCompleteWindowData;
                Func<bool> canProceed = () => !data.actorController.IsAnimatingActor() && data.dialoguePanel.IsHiding;
                
                if(chapterEndId == "end_win")
                {
                    gameCompleteWindowData = new GameCompleteWindowData() 
                    {
                        canProceed = canProceed,
                        isGameStatusCompleted = true,
                        onWindowClosed = () =>
                        {
                            data.backToTitleScene?.Invoke();
                        }
                    };
                }
                else
                {
                    gameCompleteWindowData = new GameCompleteWindowData()
                    {
                        canProceed = canProceed,
                        isGameStatusCompleted = true,
                        onWindowClosed = () =>
                        {
                            data.backToChapterSelectionScene?.Invoke();
                        }
                    };
                }
                WindowController.Instance.Show(nameof(WGameComplete), gameCompleteWindowData);
            }
            else
            {
                data.backToChapterSelectionScene?.Invoke();
            }

            Stop();
        }

        public void Stop()
        {
            IsPlaying = false;
        }
    }
}
