using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class LoadingOverlay : MonoBehaviour
    {
        [SerializeField] private Image overlayImage = default;

        public bool IsPlayingAnimation { get; private set; } = false;
        public const string OVERLAY_ANIMATION_ID = "overlay_animation_id";
        private Coroutine overlayCor = default;

        public void ShowAndHideOverlay(float fadeInDuration, Action callback, float hideDelayDuration, float fadeOutDuration)
        {
            KillAnimationCor();
            overlayCor = StartCoroutine(ShowAndHideOverlayCor(fadeInDuration, callback, hideDelayDuration, fadeOutDuration));
        }

        public void InitialLoading()
        {
            overlayImage.gameObject.SetActive(true);
            overlayImage.color = new Color32(0, 0, 0, 255);
        }

        public void HideOvelay(float fadeOutDuration)
        {
            KillAnimationCor();
            overlayCor = StartCoroutine(HideOvelayCor(fadeOutDuration));
        }

        public IEnumerator HideOvelayCor(float fadeOutDuration)
        {
            yield return overlayImage.DOFade(0f, fadeOutDuration).SetId(OVERLAY_ANIMATION_ID).WaitForCompletion();
            IsPlayingAnimation = false;
            overlayImage.gameObject.SetActive(false);
        }

        private IEnumerator ShowAndHideOverlayCor(float fadeInDuration, Action callback, float hideDelayDuration, float fadeOutDuration)
        {
            overlayImage.gameObject.SetActive(true);
            IsPlayingAnimation = true;
            yield return overlayImage.DOFade(1f, fadeInDuration).SetId(OVERLAY_ANIMATION_ID).WaitForCompletion();
            callback?.Invoke();
            yield return new WaitForSeconds(hideDelayDuration);
            yield return HideOvelayCor(fadeOutDuration);
        }

        private void KillAnimationCor()
        {
            if (overlayCor != null)
                StopCoroutine(overlayCor);

            DOTween.Kill(OVERLAY_ANIMATION_ID);
        }

        private void OnDestroy()
        {
            KillAnimationCor();
        }
    }
}
