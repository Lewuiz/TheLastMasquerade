using System;
using UnityEngine;
namespace Main
{
    [Serializable]
    public class WindowBaseData 
    {
        [HideInInspector] public string windowName;
        public WindowBase window;

        public void UpdateWindowName()
        {
            if (window == null)
                return;

            windowName = window.name;
        }
    }
}
