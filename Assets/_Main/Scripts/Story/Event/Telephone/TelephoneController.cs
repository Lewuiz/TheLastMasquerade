using UnityEngine;

namespace Main
{
    public class TelephoneController : MonoBehaviour
    {
        [SerializeField] private Telephone telephone = default;
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
            telephone.Init();
            telephoneCg.alpha = 0f;
            telephone.gameObject.SetActive(false);
        }

        public void Show()
        {
            hasMiniGame = true;
            telephone.gameObject.SetActive(true);
            telephone.Show();
        }

        public void Hide()
        {
            hasMiniGame = false;
            telephone.Hide();
        }
    }
}
