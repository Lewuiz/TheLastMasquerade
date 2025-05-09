using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Main
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup cg = default;
        [SerializeField] private TextMeshProUGUI characterNameTMP = default;
        [SerializeField] private TextMeshProUGUI characterDialogueTMP = default;
        [SerializeField] private TextMeshProUGUI dialogueStatusTMP = default;

        private bool canClicked = true;
        private Action onDialogueClick = default;
        private Func<bool> canProceedDialogue = default;
        private bool isOverrideDialogue = false;
        private Action hideActor = default;

        public bool IsPlayingAnimation { get; private set; } = false;
        public bool IsHiding { get; private set; } = false;

        public void Init(Action onDialogueClick, Func<bool> canProceedDialogue, Action hideActor)
        {
            this.onDialogueClick = onDialogueClick;
            this.canProceedDialogue = canProceedDialogue;
            this.hideActor = hideActor;

            canClicked = true;
        }

        public bool IsDialogueHiding()
        {
            return IsHiding;
        }

        public void UpdateDialoguePanel(DialogueCharacterData dialogueCharacterData)
        {
            if(IsHiding)
                Show();

            characterDialogueTMP.text = dialogueCharacterData.text;
            characterNameTMP.text = dialogueCharacterData.character;
        }

        public void Continue()
        {
            //if (isOverrideDialogue)
            //{
            //    isOverrideDialogue = false;
            //    hideActor?.Invoke();
            //    Hide();
            //    return;
            //}

            if (!canProceedDialogue.Invoke() || !canClicked || IsPlayingAnimation)
                return;

            onDialogueClick?.Invoke();
        }

        public void Hide()
        {
            StartCoroutine(HideCor());
        }

        private IEnumerator HideCor()
        {
            IsHiding = true;
            canClicked = false;
            IsPlayingAnimation = true;
            yield return cg.DOFade(0f, .2f).WaitForCompletion();
            cg.gameObject.SetActive(false);
            IsPlayingAnimation = false;
        }

        public void Show()
        {
            StartCoroutine(ShowCor());
        }

        public void Show(bool isOverriedDialog)
        {
            this.isOverrideDialogue = isOverriedDialog;
            StartCoroutine(ShowCor());
        }

        private IEnumerator ShowCor()
        {
            cg.gameObject.SetActive(true);
            canClicked = false;
            IsPlayingAnimation = true;
            yield return cg.DOFade(1f, .2f).WaitForCompletion();
            IsPlayingAnimation = false;
            canClicked = true;
            IsHiding = false;
        }
    }
}
