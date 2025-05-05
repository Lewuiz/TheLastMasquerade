using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main
{
    public class InspectItemController : MonoBehaviour
    {
        [SerializeField] private List<InspectItemGameData> inspectItemGameDataList = new List<InspectItemGameData>();
        [SerializeField] ObtainedItemPanel obtainedItemPanel = default;

        private GameObject inspectGame = default;
        private List<InspectItem> inspecItemList = new List<InspectItem>();
        private Action showDialogue = default;
        private InventoryManager inventoryManager = default;

        public bool IsGameOnGoing { get; private set; } = false;

        public void Init(Action showDialogue)
        {
            inventoryManager = GameCore.Instance.InventoryManager;
            this.showDialogue = showDialogue;
        }

        public void Load(string gameId)
        {
            IsGameOnGoing = true;
            InspectItemGameData inspectItemGameData = inspectItemGameDataList.Find(gameData => gameData.gameId == gameId);

            if (inspectItemGameData == null)
            {
                IsGameOnGoing = false;
                return;
            }

            inspectGame = Instantiate(inspectItemGameData.inspectItemGame, transform);
            inspectGame.transform.position = Vector3.zero;
            inspectGame.transform.localScale = Vector3.one;

            IntializeInspectItem();
        }

        public void ShowObtainedPanel(List<Sprite> obtainedSpriteList)
        {
            obtainedItemPanel.gameObject.SetActive(true);
            obtainedItemPanel.Show(obtainedSpriteList);
        }

        private void IntializeInspectItem()
        {
            inspecItemList = FindObjectsByType<InspectItem>(FindObjectsSortMode.None).ToList();

            for (int i = 0; i < inspecItemList.Count; i++)
            {
                inspecItemList[i].Init(OnItemFound);
            }
        }

        private void OnItemFound(InspectItem inspectItem)
        {
            for (int i = 0; i < inspectItem.InspectItemDataList.Count; i++)
            {
                inventoryManager.ObtainedInspectItem(inspectItem.InspectItemDataList[i].inspectItemId);
            }

            ShowObtainedPanel(inspectItem.GetInspectItemSpriteList());
        }
    }

    [Serializable]
    public class InspectItemGameData
    {
        public string gameId;
        public GameObject inspectItemGame = default;
    }
}
