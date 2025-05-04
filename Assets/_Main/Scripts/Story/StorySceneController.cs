using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class StorySceneController : RootSceneController
    {
        [SerializeField] private SpriteRenderer backgroundSR = default;
        [SerializeField] private BackgroundSizeFitter backgroundSizeFitter = default;
        [SerializeField] private StoryRunner storyRunner = default;
        [SerializeField] private DialogueChoicePanel dialogueChoicePanel = default;
        [SerializeField] private ActorController actorController = default;
        [SerializeField] private DialoguePanel dialoguePanel = default;
        [SerializeField] private StoryEventHandler storyEventHandler = default;
        [SerializeField] private InspectItemController inspectItemController = default;

        private StoryManager storyManager = default;
        private LoadingOverlay loadingOverlay = default;

        protected override void OnStartCompleted()
        {
            loadingOverlay = Overlay.Instance.LoadingOverlay;

            loadingOverlay.InitialLoading();
            storyManager = GameCore.Instance.StoryManager;

            actorController.Init();
            InitializeStoryRunner();
            dialoguePanel.Init(storyRunner.PlayNextDialogue, CanProceedNextDialogue);
            dialogueChoicePanel.Init();
            storyEventHandler.Init(UpdateBackground);

            loadingOverlay.HideOvelay(.5f, .1f);
        }

        private bool CanProceedNextDialogue()
        {
            return !IsPlayingAnimation() && !storyRunner.IsChapterEnded;
        }

        private void CheckActorCharacter(DialogueActorControl dialogueActorControl)
        {
            StartCoroutine(CheckActorCharacterCor(dialogueActorControl));
        }

        private void InitializeStoryRunner()
        {
            StorySceneData storySceneData = GameCore.Instance.StorySceneData;
            StoryRunnerData storyRunnerData = new StoryRunnerData()
            {
                storyManager = storyManager,
                addCharacter = actorController.AddCharacterInDialogue,
                onDialoguePlay = OnDialoguePlay,
                executeDialogueEvent = ExecuteStoryEvent,
                isAnimating = IsPlayingAnimation,
                updateDialoguePanel = dialoguePanel.UpdateDialoguePanel,
                backToChapterSelectionScene = BackToChapterSelection,
                storyChapter = storySceneData.SelectedChapter <= -1 ? storyManager.CurrentChapter : storySceneData.SelectedChapter
            };
            storyRunner.Init(storyRunnerData);
        }

        public void OnDialoguePlay(DialogueActorControl dialogueActorControl, string characterId)
        {
            StartCoroutine(OnDialoguePlayCor(dialogueActorControl, characterId));
        }

        private IEnumerator OnDialoguePlayCor(DialogueActorControl dialogueActorControl, string characterId)
        {
            yield return CheckActorCharacterCor(dialogueActorControl);
            yield return actorController.UpdateActorConversationCor(characterId);
        }

        private IEnumerator CheckActorCharacterCor(DialogueActorControl dialogueActorControl)
        {
            if (dialogueActorControl == null)
                yield break;

            var hideActorlist = dialogueActorControl.hide;
            yield return actorController.RemoveActorInDialogueCor(hideActorlist);

            var showActorlist = dialogueActorControl.show;
            yield return actorController.AddCharacterInDialogueCor(showActorlist);
        }

        private void ExecuteStoryEvent(List<DialogueEventData> dialoguesDataList)
        {
            storyEventHandler.ExecuteEvents(dialoguesDataList);
        }

        private void ShowDialogueChoice()
        {
            dialogueChoicePanel.gameObject.SetActive(true);
            dialogueChoicePanel.ShowDialogue();
        }

        private void UpdateBackground(Sprite sprite)
        {
            void callback()
            {
                backgroundSR.sprite = sprite;
                backgroundSizeFitter.FitToCamera();
            }
            loadingOverlay.ShowAndHideOverlay(.3f, callback, .5f, .3f);
        }

        private bool IsPlayingAnimation()
        {
            return actorController.IsAnimatingActor() || loadingOverlay.IsPlayingAnimation;
        }

        private void BackToChapterSelection()
        {
            loadingOverlay.Show(.5f, () =>
            {
                LoadScene(SceneID.ChapterSelectionScene);
            });
        }
    }
}
