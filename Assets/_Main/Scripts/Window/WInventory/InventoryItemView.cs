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
        public InventoryItem InventoryItem { get; private set; } = default;

        public void Init(InventoryItem inventoryItem, Action<InventoryItemView> selectInventoryItem)
        {
            InventoryItem = inventoryItem;
            this.selectInventoryItem = selectInventoryItem;
            itemIcon.sprite = inventoryItem.sprite;
            DeselectItemUI();
        }

        public void SelectItemUI()
        {
            itemBG.color = SELECTED_COLOR;
        }

        public void DeselectItemUI()
        {
            itemBG.color = UNSELECTED_COLOR;
        }

        public void OnButtonClicked()
        {
            selectInventoryItem?.Invoke(this);
        }

    }
}
