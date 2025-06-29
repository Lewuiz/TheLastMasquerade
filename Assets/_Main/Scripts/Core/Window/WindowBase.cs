using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Main
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform = default;
        [SerializeField] private CanvasGroup canvasGroup = default;

        private const float ANIMATION_DURATION = 0.5f;

        private void SetDefaultOpen()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            transform.localScale = Vector3.zero;

        }

        private void SetDefaultClose()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

        }

        public abstract void SetDefault();

        public void OpenWindow()
        {
            SetDefaultOpen();
            StartCoroutine(OpenWindowCor());
        }

        private IEnumerator OpenWindowCor()
        {  
            canvasGroup.DOFade(1f, ANIMATION_DURATION);
            yield return transform.DOScale(Vector3.one, ANIMATION_DURATION);
            OnOpenComplete();
        }

        public virtual void CloseWindow()
        {
            SetDefaultClose();
            StartCoroutine(CloseWindowCor());
        }

        private IEnumerator CloseWindowCor()
        {
            canvasGroup.DOFade(0f, ANIMATION_DURATION);
            yield return transform.DOScale(Vector3.zero, ANIMATION_DURATION);
            OnCloseComplete();
        }

        protected virtual void OnOpenComplete() {}
        protected virtual void OnCloseComplete() { }
    }
}
