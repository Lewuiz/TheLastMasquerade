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

        private StoryManager storyManager = default;
        private LoadingOverlay loadingOverlay = default;

        protected override void OnStartCompleted()
        {
            loadingOverlay = Overlay.Instance.LoadingOverlay;
            loadingOverlay.InitialLoading();

            storyManager = GameCore.Instance.StoryManager;
            storyEventHandler.Init(UpdateBackground);
            actorController.Init();
            dialoguePanel.Init();
            dialogueChoicePanel.Init();

            InitializeStoryRunner();
            storyRunner.Run();

            loadingOverlay.HideOvelay(.5f, .1f);
        }


        private void InitializeStoryRunner()
        {
            StoryRunnerData storyRunnerData = new StoryRunnerData()
            {
                storyManager = storyManager,
                actorController = actorController,
                backToChapterSelectionScene = BackToChapterSelection,
                dialogueChoicePanel = dialogueChoicePanel,
                dialoguePanel = dialoguePanel,
                storyEventHandler = storyEventHandler,
            };
            storyRunner.Init(storyRunnerData);
        }

        //private void HideDialogue()
        //{
        //    dialoguePanel.Hide();
        //    actorController.HideAllCharacter();
        //}

        //private void ShowDialogue(bool isOverrideDialogue = false)
        //{
        //    dialoguePanel.Show(isOverrideDialogue);
        //    actorController.ShowAllCharacter();
        //}

        //public void OnDialoguePlay(DialogueActorControl dialogueActorControl, string characterId)
        //{
        //    StartCoroutine(OnDialoguePlayCor(dialogueActorControl, characterId));
        //}

        //private IEnumerator OnDialoguePlayCor(DialogueActorControl dialogueActorControl, string characterId)
        //{
        //    yield return CheckActorCharacterCor(dialogueActorControl);
        //    yield return actorController.UpdateActorConversationCor(characterId);
        //}

        //private IEnumerator CheckActorCharacterCor(DialogueActorControl dialogueActorControl)
        //{
        //    if (dialogueActorControl == null)
        //        yield break;

        //    var hideActorlist = dialogueActorControl.hide;
        //    yield return actorController.RemoveActorInDialogueCor(hideActorlist);

        //    var showActorlist = dialogueActorControl.show;
        //    yield return actorController.AddCharacterInDialogueCor(showActorlist);
        //}

        //private void ExecuteStoryEvent(List<DialogueEventData> dialoguesDataList)
        //{
        //    storyEventHandler.ExecuteEvents(dialoguesDataList);
        //}

        //private void ShowDialogueChoice(List<DialogueChoiceData> dialogueChoiceDataList, Action onChoiceSelected)
        //{
        //    if (dialogueChoiceDataList == null || dialogueChoiceDataList.Count == 0)
        //        return;

        //    dialogueChoicePanel.gameObject.SetActive(true);
        //    dialogueChoicePanel.ShowChoiceDialogue(dialogueChoiceDataList, onChoiceSelected);
        //}

        private void UpdateConversation()
        {
 
        }

        private void UpdateBackground(Sprite sprite)
        {
            if (backgroundSR.sprite == sprite)
                return;

            loadingOverlay.ShowAndHideOverlay(.3f, () => 
            {
                backgroundSR.sprite = sprite;
                backgroundSizeFitter.FitToCamera();
            }, .5f, .3f);
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
