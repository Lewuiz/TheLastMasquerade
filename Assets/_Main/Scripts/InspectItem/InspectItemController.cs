using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class InspectItemController : MonoBehaviour
    {
        [SerializeField] private Image collectItemImage = default;
        [SerializeField] private CharacterData characterData = default;
        [SerializeField] private Transform spawnParent = default;

        private DialoguePanel dialoguePanel = default;
        private ActorController actorController = default;

        private List<ItemInspection> itemInspectionList = new List<ItemInspection>();

        private GameObject itemGameobject = default;
        private bool isShowing = false;

        public event Action OnItemInspectionCompleted = default;
        private InventoryManager inventoryManager = default;
        private Action forceRunStory = default;
        private List<string> claimItemList = new List<string>();
        private JigsawController jigsawController = default;
        private bool isPlayingJigsaw = default;

        public void Init(DialoguePanel dialoguePanel, ActorController actorController, Action forceRunStory, JigsawController jigsawController)
        {
            this.dialoguePanel = dialoguePanel;
            this.actorController = actorController;
            this.forceRunStory = forceRunStory;
            this.jigsawController = jigsawController;

            inventoryManager = GameCore.Instance.InventoryManager;
        }

        private void Save(string id)
        {
            inventoryManager.Claim(id);
        }

        public void Spawn(GameObject itemPrefab)
        {
            dialoguePanel.SetOnClickEvent(() => 
            {
                dialoguePanel.Hide();
                actorController.FadeCharacterOnDialogue(characterData.characterId);
                SetAllItemCanClick(true);
            });

            SetDefault();
            isShowing = true;
            itemGameobject = Instantiate(itemPrefab, spawnParent);
            itemInspectionList = itemGameobject.GetComponentsInChildren<ItemInspection>().ToList();

            for (int i = 0; i < itemInspectionList.Count; i++) 
            {
                itemInspectionList[i].Init(CollectItem, Save, ()=> 
                { 
                    isShowing = false; 
                    forceRunStory?.Invoke();
                    OnItemInspectionCompleted?.Invoke();
                }, CheckItem);
            }
            SetAllItemCanClick(false);
        }

        public void SetDefault()
        {
            if (itemGameobject != null)
            {
                Destroy(itemGameobject);
            }
            claimItemList = new List<string>();
            collectItemImage.color = new Color32(255, 255, 255, 0);
            itemInspectionList.Clear();
            isShowing = false;
        }

        private void CollectItem(ItemInspection itemInspection)
        {
            if (itemInspection.InspectionType == ItemInspectionShowingType.Window)
            {
                CollectItemWindowData collectItemWindowData = new CollectItemWindowData()
                {
                    sprite = itemInspection.InventoryItem.sprite,
                    onWindowClosed = () => 
                    {
                        StartCoroutine(PlayDialogueCor(itemInspection));
                    }

            };
                WindowController.Instance.Show(nameof(WCollectItem), collectItemWindowData);
            }
            else if(itemInspection.InspectionType == ItemInspectionShowingType.Display)
            {
                collectItemImage.sprite = itemInspection.InventoryItem.sprite;
                collectItemImage.DOFade(1f, .3f);
                StartCoroutine(PlayDialogueCor(itemInspection));
            }

            if (itemInspection.HasInteractWithKeyItem)
                itemInspection.OnInteractCompleted();       
        }

        private IEnumerator PlayDialogueCor(ItemInspection itemInspection)
        {
            SetAllItemCanClick(false);
       
            dialoguePanel.Show();
            dialoguePanel.SetCanClick(false);
            dialoguePanel.SetOnClickEvent(Continue);
            actorController.UnFadeCharacterOnDialogue(characterData.characterId);
            var selectedActor = actorController.GetActorOnDialogue(characterData.characterId);
            selectedActor.ShowConversation();
            dialoguePanel.UpdateDialoguePanel(itemInspection.InventoryItem.dialogueText, characterData.characterName);
            
            while (actorController.IsAnimating)
                yield return null;

            while (!itemInspection.HasInteractWithKeyItem)
                yield return null;

            dialoguePanel.SetCanClick(true);
        }

        private void Continue()
        {
            if (HasCollectAllInspectionItem() && !isPlayingJigsaw)
            {
                isShowing = false;
                SetDefault();
                collectItemImage.DOFade(0f, .3f);
                dialoguePanel.Show();
                actorController.UnFadeCharacterOnDialogue(characterData.characterId);
                OnItemInspectionCompleted?.Invoke();
            }
            else
            {
                dialoguePanel.Hide();
                collectItemImage.DOFade(0f, .3f);
                actorController.FadeCharacterOnDialogue(characterData.characterId);
                SetAllItemCanClick(true);
            }
        }

        private void CheckItem(string id)
        {
            claimItemList.Add(id);

            //hard coded due to limitation of time
            if(claimItemList.Count > 0)
            {
                if (claimItemList.Count == 4 && claimItemList[0].Contains("torn_paper"))
                {
                    isPlayingJigsaw = true;
                    jigsawController.SetOnCompleted(() => 
                    {
                        isPlayingJigsaw = false;
                        dialoguePanel.Show();
                        collectItemImage.DOFade(0f, .3f);
                        actorController.UnFadeCharacterOnDialogue(characterData.characterId);

                        inventoryManager.Claim("guest_list");
                        var guestList = inventoryManager.GetItem("guest_list");
                        CollectItemWindowData collectItemWindowData = new CollectItemWindowData()
                        {
                            onWindowClosed = () => 
                            { 
                                Continue(); 
                            },
                            sprite = guestList.sprite
                        };
                        WindowController.Instance.Show(nameof(WCollectItem), collectItemWindowData);
                    });
                    jigsawController.Show();
                }
            }
        }

        private void SetAllItemCanClick(bool canClick)
        {
            for (int i = 0; i < itemInspectionList.Count; i++)
            {
                itemInspectionList[i].SetCanClick(canClick);
            }
        }

        public bool IsExecuting()
        {
            return isShowing || WindowController.Instance.IsWindowOpened(nameof(WCollectItem)) || !HasCollectAllInspectionItem();
        }

        public void UnlockInspectionItem(InventoryItem inventoryItem)
        {
            for (int i = 0; i < itemInspectionList.Count; i++)
            {
                InventoryItem itemInspectionInventory = itemInspectionList[i].InteractKeyItem(inventoryItem);
                if (itemInspectionInventory != null)
                {
                    if (dialoguePanel.DialogueText == itemInspectionInventory.dialogueText)
                    {
                        Continue();
                    }
                }

            }
        }

        private bool HasCollectAllInspectionItem()
        {
            for (int i = 0; i < itemInspectionList.Count; i++)
            {
                if (!itemInspectionList[i].HasInteractWithKeyItem || !itemInspectionList[i].HasCollected)
                    return false; 
            }
            return true;
        }
    }
}
