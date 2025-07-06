using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(fileName = "Inventory Database", menuName = "Inventory/Database")]
    public class InventoryDatabase : ScriptableObject
    {
        public List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    }
}
