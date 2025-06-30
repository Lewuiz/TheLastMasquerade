using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class WInventory : WindowBase
    {
        [SerializeField] private TextMeshProUGUI itemTitleTMP = default;
        [SerializeField] private TextMeshProUGUI itemDescTMP = default;
        [SerializeField] private InventoryItemView inventoryItemViewTemplate = default;

        private List<InventoryItemView> inventoryItemViewList = new List<InventoryItemView>();
        private InventoryManager inventoryManager = default;

        private InventoryItemView selectedInventoryItemView = default;

        protected override void SetDefaultUI()
        {
            inventoryManager = GameCore.Instance.InventoryManager;
            CreateInventoryItemViews();
            SetDefault();
        }

        private void CreateInventoryItemViews()
        {
            List<InventoryItem> inventoryItemList = inventoryManager.GetInventoryItemList();

            for (int i = 0; i < inventoryItemList.Count; i++) 
            {
                InventoryItemView inventoryItemView = Instantiate(inventoryItemViewTemplate, inventoryItemViewTemplate.transform.parent);
                inventoryItemView.Init(inventoryItemList[i], SelectInventoryItem);
                inventoryItemView.gameObject.SetActive(true);
                inventoryItemViewList.Add(inventoryItemView);
            }
        }

        private void SetDefault()
        {
            if (inventoryItemViewList.Count == 0)
                return;

            selectedInventoryItemView = inventoryItemViewList[0];
            selectedInventoryItemView.SelectItemUI();
            UpdateItemInfo(selectedInventoryItemView.InventoryItem);
        }

        private void SelectInventoryItem(InventoryItemView inventoryItemView) 
        {
            if (selectedInventoryItemView == inventoryItemView)
                return;

            selectedInventoryItemView.DeselectItemUI();
            inventoryItemView.SelectItemUI();
            selectedInventoryItemView = inventoryItemView;
            UpdateItemInfo(selectedInventoryItemView.InventoryItem);
        }

        private void UpdateItemInfo(InventoryItem inventoryItem)
        {
            itemTitleTMP.text = inventoryItem.inventoryId;
            itemDescTMP.text = inventoryItem.desc;
        }

        /// <summary>
        /// This method was called by button oncick event action
        /// </summary>
        public void UseItem()
        {

        }
    }
}
