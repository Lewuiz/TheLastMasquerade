using Main.Singleton;
using UnityEngine;

namespace Main
{
    public class Overlay : PersistentMonoSingleton<Overlay>
    {
        [SerializeField] private Canvas canvas = default;
        [field: SerializeField] public LoadingOverlay LoadingOverlay { get; private set; } = default;
        
        private bool hasInit = false;

        public void Init()
        {
            hasInit = true;
            UpdateCameraAndCanvas();
        }

        private void UpdateCameraAndCanvas()
        {
            if (!hasInit)
                return;

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingLayerName = "UI";
            canvas.sortingOrder = 999;
        }
    }
}
