using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main
{
    public class InspectItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private List<InspectItemData> inspectItemDataList = new List<InspectItemData>();
        [field: SerializeField] public string PreInspectItemInformation { get; private set; } = default;
        [SerializeField] private string requiredItems = default;
        public string RequiredItems => requiredItems;
        [field: SerializeField] public string PostInspectItemInformation { get; private set; } = default;
        public List<InspectItemData> InspectItemDataList => inspectItemDataList;

        public bool hasFound= false;
        private Action<InspectItem> onItemFound = default;

        public void Init(Action<InspectItem> onItemFound)
        {
            this.onItemFound = onItemFound;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            FoudItem();
        }

        public void FoudItem()
        {
            onItemFound?.Invoke(this);
        }

        public List<Sprite> GetInspectItemSpriteList()
        {
            return inspectItemDataList.Select(item => item.sprite).ToList();
        }
    }
}
