using UnityEngine;

namespace Main
{
    public class ChapterSelectionSceneController : RootSceneController
    {
        protected override void OnStartCompleted()
        {
            Overlay.Instance.LoadingOverlay.HideOvelay(.5f, 1f);
        }
    }
}
