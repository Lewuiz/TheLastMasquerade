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
        private InspectItemGameData inspectItemGameData = default;

        private List<InspectItem> inspecItemList = new List<InspectItem>();
        private Action showDialogue = default;

        private InventoryManager inventoryManager = default;
        private StoryManager storyManager = default;

        public bool IsGameOnGoing { get; private set; } = false;
        private Action playNextDialogue = default;

        public void Init(Action showDialogue, Action playNextDialogue)
        {
            this.showDialogue = showDialogue;
            this.playNextDialogue = playNextDialogue;

            inventoryManager = GameCore.Instance.InventoryManager;
            storyManager = GameCore.Instance.StoryManager;
        }

        public void Load(string gameId)
        {
            IsGameOnGoing = true;
            inspectItemGameData = inspectItemGameDataList.Find(gameData => gameData.gameId == gameId);

            if (inspectItemGameData == null || storyManager.HasMiniGamePlayed(inspectItemGameData.gameId))
            {
                IsGameOnGoing = false;
                return;
            }

            inspectGame = Instantiate(inspectItemGameData.inspectItemGame, transform);
            inspectGame.transform.position = Vector3.zero;
            inspectGame.transform.localScale = Vector3.one;

            IntializeInspectItem();
        }

        public void ShowObtainedPanel(List<Sprite> obtainedSpriteList, Action onFindAllItem)
        {
            obtainedItemPanel.gameObject.SetActive(true);
            obtainedItemPanel.SetOnFindAllItem(onFindAllItem);
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

            inspectItem.gameObject.SetActive(false);
            ShowObtainedPanel(inspectItem.GetInspectItemSpriteList(), OnFindAllItems);
        }

        private void OnFindAllItems()
        {
            for (int i = 0; i < inspecItemList.Count; i++)
            {
                if (!inspecItemList[i].HasFound)
                    return;
            }
            Debug.Log($"Inspect game id: {inspectItemGameData.gameId}");
            storyManager.CompleteMiniGame(inspectItemGameData.gameId);
            showDialogue?.Invoke();
            IsGameOnGoing = false;
            Destroy(inspectGame.gameObject);
            inspectGame = null;
            inspectItemGameData = null;
            playNextDialogue?.Invoke();
        }
    }

    [Serializable]
    public class InspectItemGameData
    {
        public string gameId;
        public GameObject inspectItemGame = default;
    }
}
