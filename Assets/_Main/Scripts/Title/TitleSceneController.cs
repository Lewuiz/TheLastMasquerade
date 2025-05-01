using UnityEngine;

namespace Main
{
    public class TitleSceneController : RootSceneController
    {
        public void Play()
        {
            LoadScene(SceneID.ChapterSelectionScene);
        }
    }
}
