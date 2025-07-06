using UnityEngine;

namespace Main
{
    [CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory/Item")]
    public class InventoryItem : ScriptableObject
    {
        public string inventoryId;
        public string itemName;
        public string desc; 
        public Sprite sprite;
        public string dialogueText;
    }
}
