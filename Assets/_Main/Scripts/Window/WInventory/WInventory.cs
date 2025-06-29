using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main
{
    public class WInventory : WindowBase
    {
        [SerializeField] private TextMeshProUGUI itemTitleTMP = default;
        [SerializeField] private TextMeshProUGUI itemDescTMP = default;
        [SerializeField] private InventoryItemView inventoryItemView = default;

        private List<InventoryItemView> inventoryItemViewList = new List<InventoryItemView>();
        private StoryManager storyManager = default;

        private InventoryItemView selectedInventoryItemView = default;

        protected override void SetDefault()
        {
            storyManager = GameCore.Instance.StoryManager;
            CreateInventoryItemViews();
        }

        private void CreateInventoryItemViews()
        {

        }

        private void SelectInventoryItem(InventoryItemView inventoryItemView) 
        {
        
        }
    }
}
