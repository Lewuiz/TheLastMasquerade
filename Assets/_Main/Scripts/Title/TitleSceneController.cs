using UnityEngine;

namespace Main
{
    public class TitleSceneController : RootSceneController
    {
        public void Play()
        {
            void callback()
            {
                LoadScene(SceneID.ChapterSelectionScene);
            }
            Overlay.Instance.LoadingOverlay.Show(.5f, callback);
        }
    }
}
