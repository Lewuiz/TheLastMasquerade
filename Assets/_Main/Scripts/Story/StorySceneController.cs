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
        [SerializeField] private JigsawController jigsawController = default;

        private StoryManager storyManager = default;
        private LoadingOverlay loadingOverlay = default;

        protected override void OnStartCompleted()
        {
            loadingOverlay = Overlay.Instance.LoadingOverlay;
            loadingOverlay.InitialLoading();

            storyManager = GameCore.Instance.StoryManager;
            storyEventHandler.Init(UpdateBackground, inspectItemController);
            actorController.Init();
            dialoguePanel.Init();
            dialogueChoicePanel.Init(()=>
            {
                storyRunner.SetDefault();
                dialoguePanel.Continue();
            });

            InitializeStoryRunner();

            inspectItemController.Init(dialoguePanel, actorController, storyRunner.ForceProceedDialogue, jigsawController);
            DetachItemInspectionListener();
            AttachItemInspectionListener();

            jigsawController.Init();
            jigsawController.Hide();

            storyRunner.Run();

            loadingOverlay.HideOvelay(.5f, .1f);
        }

        public void ShowInventory()
        {
            InventoryWindowData inventoryWindowData = new InventoryWindowData() 
            {
                onUseItem = UseInventoryItem
            };
            WindowController.Instance.Show(nameof(WInventory), inventoryWindowData);
        }

        private void UseInventoryItem(InventoryItem inventoryItem)
        {
            inspectItemController.UnlockInspectionItem(inventoryItem);

        }

        private void AttachItemInspectionListener()
        {
            inspectItemController.OnItemInspectionCompleted += storyRunner.SetDialoguePanelEvent;
            inspectItemController.OnItemInspectionCompleted += dialoguePanel.Continue;
        }

        private void DetachItemInspectionListener()
        {
            inspectItemController.OnItemInspectionCompleted -= storyRunner.SetDialoguePanelEvent;
            inspectItemController.OnItemInspectionCompleted -= dialoguePanel.Continue;
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
                inspectItemController = inspectItemController,
            };
            storyRunner.Init(storyRunnerData);
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

        protected override void UpdateWindowCanvasCamera()
        {
            WindowController.Instance.UpdateWindowCanvasCamera();
        }

        private void OnDestroy()
        {
            DetachItemInspectionListener();
        }
    }
}
