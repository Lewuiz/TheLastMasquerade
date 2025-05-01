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
        [SerializeField] private LoadingOverlay loadingOverlay = default;
        [SerializeField] private StoryEventHandler storyEventHandler = default;

        private StoryManager storyManager = default;

        protected override void OnStartCompleted()
        {
            loadingOverlay.InitialLoading();
            storyManager = GameCore.Instance.StoryManager;

            actorController.Init();
            InitializeStoryRunner();
            dialoguePanel.Init(storyRunner.PlayNextDialogue, IsPlayingAnimation);
            dialogueChoicePanel.Init();
            storyEventHandler.Init(UpdateBackground);

            loadingOverlay.HideOvelay(1f);
        }

        private void CheckActorCharacter(DialogueActorControl dialogueActorControl)
        {
            StartCoroutine(CheckActorCharacterCor(dialogueActorControl));
        }

        private void InitializeStoryRunner()
        {
            StoryRunnerData storyRunnerData = new StoryRunnerData()
            {
                storyManager = storyManager,
                addCharacter = actorController.AddCharacterInDialogue,
                checkCharacterControl = CheckActorCharacter,
                executeDialogueEvent = ExecuteStoryEvent,
                isAnimating = IsPlayingAnimation,
                updateActorConversation = actorController.UpdateActorConversation,
                updateDialoguePanel = dialoguePanel.UpdateDialoguePanel
            };
            storyRunner.Init(storyRunnerData);
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
    }
}
