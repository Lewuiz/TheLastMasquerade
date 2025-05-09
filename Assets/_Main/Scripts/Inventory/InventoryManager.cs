using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using UnityEditor;
using UnityEngine;

namespace Main
{
    public class InventoryManager : MonoBehaviour
    {
        private SaveManager saveManager = default;
        private PlayerData.Inventory inventory = default;

        public void Init()
        {
            saveManager = GameCore.Instance.SaveManager;
            inventory = saveManager.Get<PlayerData>(PlayerData.INVENTORY).ToObject<PlayerData.Inventory>();
        }

        public void ObtainedInspectItem(string inspectItemId)
        {
            inventory.inspectItemList.Add(inspectItemId);
            Save();
        }

        public bool HasObtainedItem(string inspectItem)
        {
            return inventory.inspectItemList.Contains(inspectItem);
        }

        public void Save()
        {
            saveManager.Set<PlayerData>(PlayerData.INVENTORY, inventory);
        }
    }
}
