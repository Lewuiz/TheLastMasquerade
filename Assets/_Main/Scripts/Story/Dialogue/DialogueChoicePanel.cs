using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class DialogueChoicePanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup cg = default;
        [SerializeField] private DialogueChoice dialogueChoiceTemplate = default;
        [SerializeField] private Color32 selectedDialogueChoiceColor = default;
        [SerializeField] private Color32 defaultDialogueChoiceColor = default;

        private List<DialogueChoice> dialogueChoiceList = new List<DialogueChoice>();
        private DialogueChoice selectedDialogueChoice = default;
        private Action onChoiceSelected = default;
        private bool isShowingChoice  = false;
        private StoryManager storyManager = default;

        public void Init()
        {
            storyManager = GameCore.Instance.StoryManager;
            cg.alpha = 0f;
            SetCanvasGroupInteactable(false);
            gameObject.SetActive(false);
        }

        public bool IsShowingChoice()
        {
            return isShowingChoice;
        }

        public void ShowChoiceDialogue(List<DialogueChoiceData> dialogueChoiceDataList, Action onChoiceSelected)
        {
            this.onChoiceSelected = onChoiceSelected;

            for (int i = 0; i < dialogueChoiceDataList.Count; i++)
            {
                DialogueChoice dialogueChoice = Instantiate(dialogueChoiceTemplate, dialogueChoiceTemplate.transform.parent);
                dialogueChoice.Init(OnDialogueSelect);
                DialogueChoiceData choiceData = dialogueChoiceDataList[i];
                dialogueChoice.UpdateDialogue(choiceData.nextDialogueId, choiceData.text);
                dialogueChoice.gameObject.SetActive(true);
                dialogueChoiceList.Add(dialogueChoice);
            }

            StartCoroutine(ShowDialogueCor());
        }

        private IEnumerator ShowDialogueCor()
        {
            gameObject.SetActive(true);
            SetCanvasGroupInteactable(false);
            yield return cg.DOFade(1f, .3f).WaitForCompletion();
            SetCanvasGroupInteactable(true);
            isShowingChoice = true;
        }

        private void SetCanvasGroupInteactable(bool canInteract)
        {
            cg.interactable = canInteract;
            cg.blocksRaycasts = canInteract;
        }

        public void SubmitDialogue()
        {
            StartCoroutine(SubmitDialogueCor());
        }

        private IEnumerator SubmitDialogueCor()
        {
            SetCanvasGroupInteactable(false);
            yield return cg.DOFade(0f, .3f).WaitForCompletion();
            onChoiceSelected?.Invoke();
            DestoryChoices();
            isShowingChoice = false;
            gameObject.SetActive(false);
        }

        private void OnDialogueSelect(DialogueChoice dialogueChoice)
        {
            if (selectedDialogueChoice == null)
            {
                selectedDialogueChoice = dialogueChoice;
                selectedDialogueChoice.UpdateDialogueButton(selectedDialogueChoiceColor);
            }
            else if(selectedDialogueChoice == dialogueChoice)
            {
                storyManager.UpdateDialogueId(dialogueChoice.NextDialogueId);
                SubmitDialogue();
            }
            else
            {
                selectedDialogueChoice.UpdateDialogueButton(defaultDialogueChoiceColor);
                selectedDialogueChoice = dialogueChoice;
                dialogueChoice.UpdateDialogueButton(selectedDialogueChoiceColor);
            }
        }

        private void DestoryChoices()
        {
            for(int i = 0; i < dialogueChoiceList.Count; i++)
            {
                Destroy(dialogueChoiceList[i].gameObject);
            }

            dialogueChoiceList.Clear();
        }
    }
}
