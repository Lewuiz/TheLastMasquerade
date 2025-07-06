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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                WindowController.Instance.Show(nameof(WTelephone));
            }
        }

        protected override void UpdateWindowCanvasCamera()
        {
            WindowController.Instance.UpdateWindowCanvasCamera();
        }
    }
}
