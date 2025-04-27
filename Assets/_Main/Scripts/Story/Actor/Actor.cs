using DG.Tweening;
using UnityEngine;

namespace Main
{
    public class Actor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer actorSR = default;

        public CharacterData CharacterData { get; private set; } = default;

        public bool CanOverrideActor { get; private set; } = false;
        private int defaultSortingOrder = default;

        public void Init()
        {
            defaultSortingOrder = actorSR.sortingOrder;
        }

        public void SetCharacterData(CharacterData characterData)
        {
            SetDefault();

            this.CharacterData = characterData;
            actorSR.sprite = characterData.characterSpriteList[0];
            CanOverrideActor = false;
        }

        private void SetDefault()
        {
            transform.localScale = Vector3.one;
            actorSR.color = Color.white;
            actorSR.sortingOrder = defaultSortingOrder;
        }

        public void Show()
        {
            Color32 activeActorColor = Color.white;

            actorSR.DOColor(activeActorColor, .3f);
            actorSR.transform.DOScale(1.02f, .3f);
            actorSR.sortingOrder = defaultSortingOrder + 1;
        }

        public void Hide()
        {
            Color32 inactiveActorColor = new Color32(107,107,107,255);

            actorSR.DOColor(inactiveActorColor, .3f);
            actorSR.transform.DOScale(1f, .3f);
            actorSR.sortingOrder = defaultSortingOrder;
        }

        public void OverrideActor()
        {
            CanOverrideActor = true;
        }
    }
}
