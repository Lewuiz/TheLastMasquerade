using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main
{
    public class Inspectitem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private List<InspectItemData> inspectItemDataList = new List<InspectItemData>();

        public bool HasFound { get; private set; } = false;
        private Action<Inspectitem> onItemFound = default;

        public void Init(Action<Inspectitem> onItemFound)
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
