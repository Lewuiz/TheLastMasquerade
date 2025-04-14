using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class DialogueChoicePanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup cg = default;
        [SerializeField] private List<DialogueChoice> dialogueChoiceList = new List<DialogueChoice>();
        [SerializeField] private Color32 selectedDialogueChoiceColor = default;
        [SerializeField] private Color32 defaultDialogueChoiceColor = default;

        private DialogueChoice selectedDialogueChoice = default;

        public void Init()
        {
            for (int i = 0; i < dialogueChoiceList.Count; i++)
            {
                dialogueChoiceList[i].Init(OnDialogueSelect);
            }

            cg.alpha = 0f;
            SetCanvasGroupInteactable(false);
            gameObject.SetActive(false);
        }

        public void ShowDialogue()
        {
            StartCoroutine(ShowDialogueCor());
        }

        private IEnumerator ShowDialogueCor()
        {
            SetCanvasGroupInteactable(false);
            yield return cg.DOFade(1f, .3f).WaitForCompletion();
            SetCanvasGroupInteactable(true);
        }

        private void SetCanvasGroupInteactable(bool canInteract)
        {
            cg.interactable = canInteract;
            cg.blocksRaycasts = canInteract;
        }

        public void HideDialogue()
        {
            StartCoroutine(HideDialogueCor());
        }

        private IEnumerator HideDialogueCor()
        {
            SetCanvasGroupInteactable(false);
            yield return cg.DOFade(0f, .3f).WaitForCompletion();
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
                SubmitDialogue();
            }
            else
            {
                selectedDialogueChoice.UpdateDialogueButton(defaultDialogueChoiceColor);
                selectedDialogueChoice = dialogueChoice;
                dialogueChoice.UpdateDialogueButton(selectedDialogueChoiceColor);
            }
        }

        private void SubmitDialogue()
        {
            HideDialogue();
        }

    }
}
