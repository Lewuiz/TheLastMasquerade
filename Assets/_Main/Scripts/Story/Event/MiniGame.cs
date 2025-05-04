using UnityEngine;

namespace Main
{
    public abstract class MiniGame : MonoBehaviour
    {
        protected bool canProceed = false;
        public bool CanProceed => canProceed;

        protected void OnReadyToProceed()
        {
            canProceed = true;
        }
    }
}
