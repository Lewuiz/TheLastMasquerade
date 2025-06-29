using System;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField] private Image itemBG = default;
        [SerializeField] private Image itemIcon = default;

        public readonly Color32 SELECTED_COLOR = new Color32(204, 139, 64, 255);
        public readonly Color32 UNSELECTED_COLOR = new Color32(217, 217, 217, 128);

        private Action<InventoryItemView> selectInventoryItem = default;

        public void Init(Action<InventoryItemView> selectInventoryItem)
        {
            this.selectInventoryItem = selectInventoryItem;
        }

        public void SelectItem()
        {
            itemBG.color = SELECTED_COLOR;
        }

        public void DeselectItem()
        {
            itemBG.color = UNSELECTED_COLOR;
        }

        public void OnButtonClicked()
        {
            selectInventoryItem?.Invoke(this);
        }

    }
}
