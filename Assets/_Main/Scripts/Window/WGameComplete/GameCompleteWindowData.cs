using System;
using UnityEngine;

namespace Main
{
    public class GameCompleteWindowData
    {
        public Func<bool> canProceed = default;
        public bool isGameStatusCompleted = default;
        public Action onWindowClosed = default;
    }
}
