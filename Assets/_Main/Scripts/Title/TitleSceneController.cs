using Papae.UnitySDK.Managers;
using UnityEngine;

namespace Main
{
    public class TitleSceneController : RootSceneController
    {
        protected override void OnStartCompleted()
        {
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/bgm_title");
            AudioManager.Instance.PlayBGM(audioClip, MusicTransition.Swift);
        }

        public void Play()
        {
            Overlay.Instance.LoadingOverlay.Show(.5f, () => 
            {
                LoadScene(SceneID.ChapterSelectionScene);
            });
        }

        public void Quit()
        {
            Application.Quit();
        }

        protected override void UpdateWindowCanvasCamera()
        {
            WindowController.Instance.UpdateWindowCanvasCamera();
        }
    }
}
