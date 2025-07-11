using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class WGameComplete : WindowBase
    {
        [SerializeField] Sprite levelCompleteSprite = default;
        [SerializeField] Sprite levelFailedSprite = default;

        [SerializeField] private Image levelImageStemple = default;

        private GameCompleteWindowData gameCompleteWindowData = default;

        private bool canProceed = false;

        protected override void SetDefaultUI()
        {
            gameCompleteWindowData = data as GameCompleteWindowData;
            levelImageStemple.transform.localScale = Vector3.zero;
            levelImageStemple.color = new Color32(255, 255, 255, 0);
            levelImageStemple.sprite = gameCompleteWindowData.isGameStatusCompleted ? levelCompleteSprite : levelFailedSprite;
            canProceed = false;
        }

        protected override void OnOpenComplete()
        {
            StartCoroutine(PlayAnimation());   
        }

        private IEnumerator PlayAnimation()
        {
            while(!gameCompleteWindowData.canProceed.Invoke())
                yield return null;

            levelImageStemple.DOFade(1f, .5f);
            yield return levelImageStemple.transform.DOScale(1f, .3f).WaitForCompletion();
            canProceed = true;
        }

        public void Continue()
        {
            if (!canProceed)
                return;

            gameCompleteWindowData.onWindowClosed?.Invoke();
            CloseWindow();
        }
    }
}
