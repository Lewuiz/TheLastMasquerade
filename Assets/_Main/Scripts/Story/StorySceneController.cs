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

        private StoryManager storyManager = default;

        protected override void OnStartCompleted()
        {
            storyManager = GameCore.Instance.StoryManager;

            actorController.Init();
            storyRunner.Init(storyManager, dialoguePanel.UpdateDialoguePanel, actorController.UpdateActorConversation, CheckActorCharacter, actorController.AddCharacterInDialogue, IsPlayingAnimation);
            dialoguePanel.Init(storyRunner.PlayNextDialogue, IsPlayingAnimation);
            dialogueChoicePanel.Init();
        }

        private void CheckActorCharacter(DialogueActorControl dialogueActorControl)
        {
            StartCoroutine(CheckActorCharacterCor(dialogueActorControl));
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

        private void ShowDialogueChoice()
        {
            dialogueChoicePanel.gameObject.SetActive(true);
            dialogueChoicePanel.ShowDialogue();
        }

        private void UpdateBackground(Sprite sprite)
        {
            backgroundSR.sprite = sprite;
            backgroundSizeFitter.FitToCamera();
        }

        private bool IsPlayingAnimation()
        {
            return actorController.IsAnimatingActor();
        }
    }
}
