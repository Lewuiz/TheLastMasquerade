using Papae.UnitySDK.Managers;
using UnityEngine;

namespace Main
{
    public class ChapterSelectionSceneController : RootSceneController
    {
        [SerializeField] private ChapterSelection chapterSelection = default;

        private LoadingOverlay loadingOverlay = default;

        protected override void OnStartCompleted()
        {
            Overlay.Instance.LoadingOverlay.InitialLoading();
            Overlay.Instance.LoadingOverlay.HideOvelay(.5f, 1f);

            chapterSelection.Init(GoToStoryScene);

            AudioClip audioClip = Resources.Load<AudioClip>("Audio/bgm_chapter_selection");
            AudioManager.Instance.PlayBGM(audioClip, MusicTransition.CrossFade);
        }

        protected override void UpdateWindowCanvasCamera()
        {
            WindowController.Instance.UpdateWindowCanvasCamera();
            loadingOverlay = Overlay.Instance.LoadingOverlay;
        }

        private void GoToStoryScene()
        {
            loadingOverlay.Show(.5f, () =>
            {
                LoadScene(SceneID.StoryScene);
            });
        }

        public void BackToTitleScene()
        {
            loadingOverlay.Show(.5f, () =>
            {
                LoadScene(SceneID.TitleScene);
            });
        }
    }
}
