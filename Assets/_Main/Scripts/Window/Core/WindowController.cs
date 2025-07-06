using Main.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [ExecuteAlways]
    public class WindowController : PersistentMonoSingleton<WindowController>
    {
        [SerializeField] private Canvas canvas = default;
        [SerializeField] private List<WindowBaseData> windows = new List<WindowBaseData>();

        private Dictionary<string, WindowBase> activeWindows = new Dictionary<string, WindowBase>();

        private void OnValidate()
        {
#if UNITY_EDITOR
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].UpdateWindowName();
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        public void Init() { }

        public void Show(string windowName, object data = null)
        {
            if (activeWindows.ContainsKey(windowName))
            {
                Debug.Log($"Window {windowName} sudah aktif.");
                return;
            }

            WindowBaseData windowData = windows.Find(w => w.windowName == windowName);
            if (windowData == null || windowData.window == null)
            {
                Debug.LogWarning($"Window {windowName} tidak ditemukan.");
                return;
            }

            WindowBase windowInstance = Instantiate(windowData.window, canvas.transform);
            windowInstance.OpenWindow(data);

            Action removeWindow = () =>
            {
                activeWindows.Remove(windowName);
            };
            windowInstance.OnWindowClosed -= removeWindow;
            windowInstance.OnWindowClosed += removeWindow;

            activeWindows.Add(windowName, windowInstance);
        }

        public void Close(string windowName)
        {
            if (activeWindows.TryGetValue(windowName, out var window))
            {
                window.CloseWindow();
                Destroy(window.gameObject);
                activeWindows.Remove(windowName);
            }
        }

        public void CloseAll()
        {
            foreach (var window in activeWindows.Values)
            {
                window.CloseWindow();
                Destroy(window.gameObject);
            }
            activeWindows.Clear();
        }

        public bool IsWindowOpened(string windowName)
        {
            return activeWindows.ContainsKey(windowName);
        }

        public void UpdateWindowCanvasCamera()
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "Windows";
            canvas.sortingOrder = 10;
        }
    }
}
