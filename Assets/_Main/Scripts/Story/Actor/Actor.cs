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
        private readonly Color32 inactiveActorColor = new Color32(107, 107, 107, 255);
        
        public CharacterData CharacterData { get; private set; } = default;
        public bool IsPlayingAnimation { get; private set; } = false;
        private string ActorAnimationID => gameObject.GetInstanceID().ToString();

        public void Init()
        {
            if (hasInit)
                return;

            hasInit = true;
            defaultSortingOrder = actorSR.sortingOrder;
            SetDefault();
        }

        public void UpdateCharacter(CharacterInCharge characterInCharge)
        {
            SetDefault();

            var characterData = characterInCharge.characterData;
            CharacterData = characterData;

            UpdateCharacterExpression(characterInCharge);
        }

        public void UpdateCharacterExpression(CharacterInCharge characterInCharge)
        {
            var characterData = characterInCharge.characterData;
            var characterState = characterData.characterSpriteList.Find(data => data.expression == characterInCharge.expression);
            if (characterState == null)
            {
                actorSR.sprite = characterData.characterSpriteList[0].sprite;
            }
            else
            {
                actorSR.sprite = characterState.sprite;
            }
        }

        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
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
            gameObject.SetActive(true);
            StartCoroutine(ShowCor());
        }

        private IEnumerator ShowCor()
        {
            IsPlayingAnimation = true;
            actorSR.transform.DOScale(1f, .2f).SetId(ActorAnimationID);
            yield return actorSR.DOFade(1f, .2f).SetId(ActorAnimationID).WaitForCompletion();
            IsPlayingAnimation = false;
        }

        public void Hide()
        {
            StartCoroutine(HideCor());
        }

        public void Fade()
        {
            actorSR.DOFade(0f, .2f);
        }

        public void UnFade()
        {
            actorSR.DOFade(1f, .2f);
        }

        public IEnumerator HideCor()
        {
            IsPlayingAnimation = true;
            actorSR.transform.DOScale(0f, .2f).SetId(ActorAnimationID);
            yield return actorSR.DOFade(0f, .2f).SetId(ActorAnimationID).WaitForCompletion();
            gameObject.SetActive(false);
            IsPlayingAnimation = false;
            CharacterData = null;
        }

        public void ShowConversation()
        {
            if (CharacterData == null)
                return;

            gameObject.SetActive(true);
            StartCoroutine(ShowConversationCor());
        }

        private IEnumerator ShowConversationCor()
        {
            IsPlayingAnimation = true;
            actorSR.sortingOrder = defaultSortingOrder + 1;
            Color32 activeActorColor = Color.white;
            actorSR.DOColor(activeActorColor, .3f).SetId(ActorAnimationID);
            yield return actorSR.transform.DOScale(1.02f, .3f).SetId(ActorAnimationID).WaitForCompletion();
            IsPlayingAnimation = false;
        }

        public void HideConversation()
        {
            if (CharacterData == null)
                return;

            gameObject.SetActive(true);
            StartCoroutine(HideConversationCor());
        }

        private IEnumerator HideConversationCor()
        {
            IsPlayingAnimation = true;
            actorSR.DOColor(inactiveActorColor, .3f).SetId(ActorAnimationID);
            yield return actorSR.transform.DOScale(1f, .3f).SetId(ActorAnimationID).WaitForCompletion();
            actorSR.sortingOrder = defaultSortingOrder;
            IsPlayingAnimation = false;
        }

        private void OnDestroy()
        {
            DOTween.Kill(ActorAnimationID);
        }
    }
}
