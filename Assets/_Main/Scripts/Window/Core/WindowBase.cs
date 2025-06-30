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
            transform.localScale = Vector3.one;

        }

        private void SetDefaultClose()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

        }

        protected abstract void SetDefaultUI();

        public void OpenWindow()
        {
            SetDefaultOpen();
            SetDefaultUI();
            StartCoroutine(OpenWindowCor());
        }

        private IEnumerator OpenWindowCor()
        {
            yield return canvasGroup.DOFade(1f, ANIMATION_DURATION).SetEase(Ease.OutQuad).WaitForCompletion();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            OnOpenComplete();
        }

        public virtual void CloseWindow()
        {
            SetDefaultClose();
            StartCoroutine(CloseWindowCor());
        }

        private IEnumerator CloseWindowCor()
        {
            yield return canvasGroup.DOFade(0f, ANIMATION_DURATION).SetEase(Ease.InQuad).WaitForCompletion();
            OnCloseComplete();
        }

        protected virtual void OnOpenComplete() {}
        protected virtual void OnCloseComplete() { }
    }
}
