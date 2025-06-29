using UnityEngine;

namespace Main
{
    public class TitleSceneController : RootSceneController
    {
        public void Play()
        {
            Overlay.Instance.LoadingOverlay.Show(.5f, () => 
            {
                LoadScene(SceneID.ChapterSelectionScene);
            });
        }

        protected override void UpdateWindowCanvasCamera()
        {
            WindowController.Instance.UpdateWindowCanvasCamera();
        }
    }
}
