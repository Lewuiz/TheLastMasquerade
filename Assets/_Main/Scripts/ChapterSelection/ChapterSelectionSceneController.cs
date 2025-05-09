using UnityEngine;

namespace Main
{
    public class ChapterSelectionSceneController : RootSceneController
    {
        [SerializeField] private ChapterSelection chapterSelection = default;
        protected override void OnStartCompleted()
        {
            Overlay.Instance.LoadingOverlay.InitialLoading();
            Overlay.Instance.LoadingOverlay.HideOvelay(.5f, 1f);

            chapterSelection.Init(GoToStoryScene);
        }

        private void GoToStoryScene()
        {
            LoadScene(SceneID.StoryScene);
        }
    }
}
