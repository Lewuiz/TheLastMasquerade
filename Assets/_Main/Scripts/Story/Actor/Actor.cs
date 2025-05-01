using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class Actor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer actorSR = default;

        private int defaultSortingOrder = default;
        private bool hasInit = false;
        public CharacterData CharacterData { get; private set; } = default;
        public bool IsInDialogue = false;

        private readonly Color32 inactiveActorColor = new Color32(107, 107, 107, 255);
        public bool IsPlayingAnimation = false;

        public void Init()
        {
            if (hasInit)
                return;

            hasInit = true;
            defaultSortingOrder = actorSR.sortingOrder;
            SetDefault();
        }

        public void SetCharacterData(CharacterData characterData)
        {
            SetDefault();

            CharacterData = characterData;
            actorSR.sprite = characterData.characterSpriteList[0];
        }

        private void SetDefault()
        {
            transform.localScale = Vector3.one;
            actorSR.color = Color.white;
            actorSR.sortingOrder = defaultSortingOrder;

            actorSR.transform.localScale = Vector3.zero;
            actorSR.color = new Color32(inactiveActorColor.r, inactiveActorColor.g, inactiveActorColor.b, 0);
        }

        public void Show()
        {
            if (IsInDialogue)
                return;

            gameObject.SetActive(true);
            StartCoroutine(ShowCor());
        }

        private IEnumerator ShowCor()
        {
            IsPlayingAnimation = true;
            actorSR.transform.DOScale(1f, .2f);
            yield return actorSR.DOFade(1f, .2f).WaitForCompletion();
            IsPlayingAnimation = false;
        }

        public void Hide()
        {
            StartCoroutine(HideCor());
        }

        public IEnumerator HideCor()
        {
            IsPlayingAnimation = true;
            actorSR.transform.DOScale(0f, .2f);
            yield return actorSR.DOFade(0f, .2f).WaitForCompletion();
            gameObject.SetActive(false);
            IsPlayingAnimation = false;
            SetActorInDialogue(false);
        }

        public void ShowConversation()
        {
            StartCoroutine(ShowConversationCor());
        }

        private IEnumerator ShowConversationCor()
        {
            IsPlayingAnimation = true;
            actorSR.sortingOrder = defaultSortingOrder + 1;
            Color32 activeActorColor = Color.white;
            actorSR.DOColor(activeActorColor, .3f);
            yield return actorSR.transform.DOScale(1.02f, .3f).WaitForCompletion();
            IsPlayingAnimation = false;
        }

        public void HideConversation()
        {
            StartCoroutine(HideConversationCor());
        }

        private IEnumerator HideConversationCor()
        {
            IsPlayingAnimation = true;
            actorSR.DOColor(inactiveActorColor, .3f);
            yield return actorSR.transform.DOScale(1f, .3f).WaitForCompletion();
            actorSR.sortingOrder = defaultSortingOrder;
            IsPlayingAnimation = false;
        }

        public void SetActorInDialogue(bool isInDialogue)
        {
            IsInDialogue = isInDialogue;
        }
    }
}
