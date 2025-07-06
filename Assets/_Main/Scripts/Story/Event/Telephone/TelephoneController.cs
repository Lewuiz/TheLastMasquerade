using UnityEngine;

namespace Main
{
    public class TelephoneController : MonoBehaviour
    {
        [SerializeField] private WTelephone telephone = default;
        [SerializeField] private CanvasGroup telephoneCg = default;

        private bool hasMiniGame = false;

        public bool HasMiniGame()
        {
            if (telephone.HasWin)
                hasMiniGame = false;

            return hasMiniGame;
        }

        public void Init()
        {
            telephoneCg.alpha = 0f;
            telephone.gameObject.SetActive(false);
        }

        public void Show()
        {
            hasMiniGame = true;
            telephone.gameObject.SetActive(true);
            telephone.Show();
        }
    }
}
