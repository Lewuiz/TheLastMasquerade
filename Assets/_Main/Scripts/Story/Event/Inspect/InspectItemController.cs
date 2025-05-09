using DG.Tweening;
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
        private Action<bool> showDialogue = default;

        private InventoryManager inventoryManager = default;
        private StoryManager storyManager = default;

        public bool IsGameOnGoing { get; private set; } = false;
        private Action playNextDialogue = default;
        private Action<DialogueCharacterData> overrideDialog = default;
        private Func<bool> isDialogueHiding = default;

        public void Init(Action<bool> showDialogue, Action playNextDialogue, Action<DialogueCharacterData> overrideDialog, Func<bool> isDialogueHiding)
        {
            this.showDialogue = showDialogue;
            this.playNextDialogue = playNextDialogue;
            this.overrideDialog = overrideDialog;
            this.isDialogueHiding = isDialogueHiding;

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

        public void ShowObtainedPanel(List<Sprite> obtainedSpriteList, Action onFindAllItem, Action showDialogue)
        {
            obtainedItemPanel.gameObject.SetActive(true);
            obtainedItemPanel.SetOnFindAllItem(onFindAllItem);
            obtainedItemPanel.Show(obtainedSpriteList, showDialogue);
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
            bool canObtain = true;
            if (!string.IsNullOrEmpty(inspectItem.RequiredItems) && !string.IsNullOrEmpty(inspectItem.RequiredItems))
            {
                
                List<string> splits = inspectItem.RequiredItems.Split(",").ToList();
                for (int i = 0; i < splits.Count; i++)
                {
                    bool hasObtainedItem = inventoryManager.HasObtainedItem(splits[i]);
                    if (!hasObtainedItem)
                    {
                        canObtain = false;
                        break;
                    }
                }
            }

            if (canObtain) 
            {
                for (int i = 0; i < inspectItem.InspectItemDataList.Count; i++)
                {
                    inventoryManager.ObtainedInspectItem(inspectItem.InspectItemDataList[i].inspectItemId);
                }
            }

            if (canObtain)
            {
                inspectItem.gameObject.SetActive(false);
                inspectItem.hasFound = true;
                ShowObtainedPanel(inspectItem.GetInspectItemSpriteList(), OnFindAllItems, () => ShowInspectItemDialog(inspectItem, true));
            }
            else
            {
                ShowInspectItemDialog(inspectItem, false);
            }
        }

        private void ShowInspectItemDialog(InspectItem inspectItem, bool hasObtainedItem)
        {
            inventoryManager = GameCore.Instance.InventoryManager;
            if (!string.IsNullOrWhiteSpace(inspectItem.PreInspectItemInformation))
            {
                DialogueCharacterData dialogueChaaracterData = new DialogueCharacterData()
                {
                    text = inspectItem.PreInspectItemInformation
                };
                overrideDialog?.Invoke(dialogueChaaracterData);
                showDialogue?.Invoke(true);
            }

            if (!string.IsNullOrWhiteSpace(inspectItem.PostInspectItemInformation) && hasObtainedItem)
            {
                DialogueCharacterData dialogueChaaracterData = new DialogueCharacterData()
                {
                    text = inspectItem.PostInspectItemInformation
                };

                overrideDialog?.Invoke(dialogueChaaracterData);
                showDialogue?.Invoke(true);
            }
        }

        private void OnFindAllItems()
        {
            for (int i = 0; i < inspecItemList.Count; i++)
            {
                if (!inspecItemList[i].hasFound)
                    return;
            }

            storyManager.CompleteMiniGame(inspectItemGameData.gameId);
            IsGameOnGoing = false;
            Destroy(inspectGame.gameObject);
            inspectGame = null;
            inspectItemGameData = null;
            bool isDialogueHide = isDialogueHiding?.Invoke() ?? false;
            showDialogue?.Invoke(false);
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
