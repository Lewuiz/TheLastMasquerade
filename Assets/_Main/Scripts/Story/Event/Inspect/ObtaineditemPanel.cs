using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class ObtainedItemPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private Image itemImageTemplate = default;

        private List<Image> obtainedImageList = new List<Image>();

        public bool IsShowingPanel { get; private set; } = default;
        private bool isPlayingAnimation = false;
        private Action onFindAllItem = default;
        private Action showDialogue = default;

        public void SetOnFindAllItem(Action onFindAllItem)
        {
            this.onFindAllItem = onFindAllItem;
        }

        public void Show(List<Sprite> obtainedItemSprite, Action showDialogue)
        {
            this.showDialogue = showDialogue;

            for (int i = 0; i < obtainedItemSprite.Count; i++)
            {
                Image itemImage = Instantiate(itemImageTemplate, itemImageTemplate.transform.parent);
                itemImage.sprite = obtainedItemSprite[i];
                obtainedImageList.Add(itemImage);
                itemImage.gameObject.SetActive(true);
            }

            StartCoroutine(ShowCor());
        }

        private IEnumerator ShowCor()
        {
            IsShowingPanel = true;
            isPlayingAnimation = true;
            yield return canvasGroup.DOFade(1f, .3f).WaitForCompletion();
            isPlayingAnimation = false;
        }

        public void Hide()
        {
            StartCoroutine(HideCor());
        }

        public IEnumerator HideCor()
        {
            isPlayingAnimation = true;
            yield return canvasGroup.DOFade(0f, .3f).WaitForCompletion();
            IsShowingPanel = false;

            for (int i = 0; i < obtainedImageList.Count; i++)
            {
                Destroy(obtainedImageList[i].gameObject);
            }

            onFindAllItem?.Invoke();
            obtainedImageList.Clear();
            isPlayingAnimation = false;
            gameObject.SetActive(false);
            showDialogue?.Invoke();
        }

        public void Continue()
        {
            if (isPlayingAnimation)
                return;

            Hide();
        }
    }
}
