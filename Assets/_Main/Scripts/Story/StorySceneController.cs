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

            storyRunner.Init(storyManager, dialoguePanel.UpdateDialoguePanel);
            dialoguePanel.Init(storyRunner.PlayNextDialogue);
            dialogueChoicePanel.Init();
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
    }
}
