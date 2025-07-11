using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main
{
    public class ItemInspection : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ItemInspectionShowingType inspectionType = default;
        [SerializeField] private InventoryItem inventoryItem = default;
        [SerializeField] private InventoryItem keyItem = default;
        [SerializeField] private string keyItemUnlockEvent = default;

        public ItemInspectionShowingType InspectionType => inspectionType;
        public InventoryItem InventoryItem => inventoryItem;

        private Action<ItemInspection> collect = default;
        private Action<string> save = default;
        public bool HasCollected { get; private set; } = default;
        public bool HasInteractWithKeyItem { get; private set; } = false;
        public bool CanClick { get; private set; } = default;

        private Action onCloseWindowEventType = default;
        private Action<string> checkItem = default;

        public void Init(Action<ItemInspection> collect, Action<string> save, Action onCloseWindowEventType, Action<string> claimItemList)
        {
            this.collect = collect;
            this.save = save;
            this.onCloseWindowEventType = onCloseWindowEventType;
            this.checkItem = claimItemList;

            HasCollected = false;
            CanClick = true;

            HasInteractWithKeyItem = keyItem == null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (HasCollected || !CanClick)
                return;

            Collect();
        }

        private void Collect()
        {
            collect?.Invoke(this);
        }

        public void SetCanClick(bool canClick)
        {
            CanClick = canClick;
        }

        public InventoryItem InteractKeyItem(InventoryItem item)
        {
            if (keyItem == null || item != keyItem)
                return null;

            HasInteractWithKeyItem = true;
            OnInteractCompleted();

            return inventoryItem;
        }

        public void OnInteractCompleted()
        {
            HasCollected = true;
            if (string.IsNullOrEmpty(keyItemUnlockEvent))
                return;

            string[] splits = keyItemUnlockEvent.Split(":");
            if (splits[0] == "window")
            {
                string windowName = splits[1];
                ShowWindow(windowName);
            }
            else if (splits[0] == "save")
            {
                string[] ids = splits[1].Split(",");

                for (int i = 0; i < ids.Length; i++)
                {
                    save?.Invoke(ids[i]);
                }
            }
            else if (splits[0] == "claim")
            {
                string id = splits[1];
                checkItem?.Invoke(id);
            }            
            //else if (splits[0] == "collect")
            //{
            //    CollectItemWindowData collectItemWindowData = new CollectItemWindowData()
            //    {
            //        inventoryItem = keyItemList[0]
            //    };
            //    WindowController.Instance.Show(nameof(WCollectItem), collectItemWindowData);
            //}
        }

        private void ShowWindow(string windowName)
        {
            if(windowName == "WTelephone")
            {
                TelephoneWindowData telephoneWindowData = new TelephoneWindowData() 
                {
                    onGameCompleted = onCloseWindowEventType
                };
                WindowController.Instance.Show(nameof(WTelephone),telephoneWindowData);
            }
        }
    }
}