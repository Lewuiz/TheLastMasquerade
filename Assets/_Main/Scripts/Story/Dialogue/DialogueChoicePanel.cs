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
        private Action<string> onChoiceSelected = default;
        private bool isShowingChoice  = false;
        private Action continueDialogue = default;

        public void Init(Action continueDialogue)
        {
            this.continueDialogue = continueDialogue;

            cg.alpha = 0f;
            SetCanvasGroupInteactable(false);
            gameObject.SetActive(false);
        }

        public bool IsShowingChoice()
        {
            return isShowingChoice;
        }

        public void ShowChoiceDialogue(List<DialogueChoiceData> dialogueChoiceDataList, Action<string> onChoiceSelected)
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

            gameObject.SetActive(true);
            StartCoroutine(ShowDialogueCor());
        }

        private IEnumerator ShowDialogueCor()
        {
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

        public void SubmitDialogue(string dialogueId)
        {
            StartCoroutine(SubmitDialogueCor(dialogueId));
        }

        private IEnumerator SubmitDialogueCor(string dialogueId)
        {
            SetCanvasGroupInteactable(false);
            yield return cg.DOFade(0f, .3f).WaitForCompletion();
            onChoiceSelected?.Invoke(dialogueId);
            DestoryChoices();
            continueDialogue?.Invoke();
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
                SubmitDialogue(dialogueChoice.NextDialogueId);
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
