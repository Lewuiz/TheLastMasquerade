using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main
{
    public class InspectItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private List<InspectItemData> inspectItemDataList = new List<InspectItemData>();

        public bool HasFound { get; private set; } = false;
        private Action<InspectItem> onItemFound = default;

        public void Init(Action<InspectItem> onItemFound)
        {
            this.onItemFound = onItemFound;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HasFound = true;
            onItemFound?.Invoke(this);
        }
    }
}
