using Main.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [ExecuteAlways]
    public class WindowController : PersistentMonoSingleton<WindowController>
    {
        [SerializeField] private Canvas canvas = default;
        [SerializeField] private List<WindowBaseData> windows = new List<WindowBaseData>();

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying)
                return;

            for (int i = 0; i < windows.Count; i++) 
            {
                windows[i].UpdateWindowName();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif

        public void Init()
        {

        }

        public void Show(string windowName)
        {
            WindowBaseData windowData = windows.Find(data => data.windowName == windowName);
            WindowBase windowBase = Instantiate(windowData.window, canvas.transform);
            windowBase.OpenWindow();
        }

        public void UpdateWindowCanvasCamera()
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
        }

        public void CloseAll()
        {
            foreach (var window in windows)
                window.window.CloseWindow();
        }
    }
}
