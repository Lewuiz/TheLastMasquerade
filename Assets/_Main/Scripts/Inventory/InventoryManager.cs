using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryDatabase database = default;

        private SaveManager saveManager = default;
        private PlayerData.Inventory inventorySaveData = default;

        public void Init()
        {
            saveManager = GameCore.Instance.SaveManager;
            inventorySaveData = saveManager.Get<PlayerData>(PlayerData.INVENTORY).ToObject<PlayerData.Inventory>();
        }

        public List<InventoryItem> GetInventoryItemList()
        {
            return database.inventoryItemList;
        }

        public bool HasObtained(string item)
        {
            return inventorySaveData.inventoryItemList.Contains(item);
        }

        public void Claim(string item)
        {
            if (HasObtained(item))
                return;

            inventorySaveData.inventoryItemList.Add(item);
            Save();
        }

        //public void ObtainedInspectItem(string inspectItemId)
        //{
        //    inventory.inspectItemList.Add(inspectItemId);
        //    Save();
        //}

        //public bool HasObtainedItem(string inspectItem)
        //{
        //    return inventory.inspectItemList.Contains(inspectItem);
        //}

        public void Save()
        {
            saveManager.Set<PlayerData>(PlayerData.INVENTORY, inventorySaveData);
        }
    }
}
