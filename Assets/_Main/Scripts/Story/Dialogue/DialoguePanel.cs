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
        [SerializeField] private TextMeshProUGUI titleTMP = default;
        [SerializeField] private TextMeshProUGUI dialogueTextTMP = default;
        [SerializeField] private TextMeshProUGUI dialogueStatusTMP = default;

        private bool canClicked = true;
        private bool isPlayingAnimation = false;

        public string DialogueText { get; private set; } = default;
        public bool IsHiding { get; private set; } = false;

        private Action onDialogueClick = default;

        public void Init()
        {
            canClicked = true;
        }

        public void SetCanClick(bool canClicked)
        {
            this.canClicked = canClicked;
        }

        public void SetOnClickEvent(Action onDialogueClick)
        {
            this.onDialogueClick = onDialogueClick;
        }

        public void UpdateDialoguePanel(string text, string title = "(Narrator)")
        {
            if (IsHiding)
                Show();
            DialogueText = text;
            dialogueTextTMP.text = text;    
            titleTMP.text = title;
        }

        public void Continue()
        {
            if (!canClicked || isPlayingAnimation)
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
            isPlayingAnimation = true;
            yield return cg.DOFade(0f, .2f).WaitForCompletion();
            cg.gameObject.SetActive(false);
            isPlayingAnimation = false;
        }

        public void Show()
        {
            StartCoroutine(ShowCor());
        }

        private IEnumerator ShowCor()
        {
            cg.gameObject.SetActive(true);
            canClicked = false;
            isPlayingAnimation = true;
            yield return cg.DOFade(1f, .2f).WaitForCompletion();
            isPlayingAnimation = false;
            canClicked = true;
            IsHiding = false;
        }
    }
}
