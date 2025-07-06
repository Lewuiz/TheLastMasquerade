using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class WCollectItem : WindowBase
    {
        [SerializeField] private Image collectItemImage = default;
        [SerializeField] private TextMeshProUGUI titleTMP = default;

        private CollectItemWindowData collectItemWindowData = default;
        private bool isPlayingAnimation = false;

        protected override void SetDefaultUI()
        {
            isPlayingAnimation = true;
            collectItemImage.color = new Color32(255, 255, 255, 0);
            collectItemWindowData = data as CollectItemWindowData;
            collectItemImage.sprite = collectItemWindowData.sprite;
        }

        protected override void OnOpenComplete()
        {
            PlayCollectItemAnimation();
        }

        private void PlayCollectItemAnimation()
        {
            collectItemImage.DOFade(1f, .4f).OnComplete(() => 
            {
                isPlayingAnimation = false;
            });
        }

        public override void CloseWindow()
        {
            if (isPlayingAnimation)
                return;

            base.CloseWindow();
            collectItemWindowData.onWindowClosed?.Invoke();

        }
    }
}
