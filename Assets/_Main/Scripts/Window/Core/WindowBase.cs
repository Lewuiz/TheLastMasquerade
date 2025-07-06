using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Main
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform = default;
        [SerializeField] private CanvasGroup canvasGroup = default;

        public event Action OnWindowClosed = default;

        private const float ANIMATION_DURATION = 0.5f;
        protected object data = default;

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

        public void OpenWindow(object data = null)
        {
            this.data = data;
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
            OnWindowClosed?.Invoke();
            OnCloseComplete();
            Destroy(this.gameObject);
        }

        protected virtual void OnOpenComplete() {}
        protected virtual void OnCloseComplete() { }
    }
}
