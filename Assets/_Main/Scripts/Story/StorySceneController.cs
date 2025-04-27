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

        private StoryManager storyManager = default;

        protected override void OnStartCompleted()
        {
            storyManager = GameCore.Instance.StoryManager;

            storyRunner.Init(storyManager, CheckDialogueEvent);
            actorController.Init();
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

        private void CheckDialogueEvent()
        {

        }
    }
}
