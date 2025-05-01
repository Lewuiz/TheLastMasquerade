using System;
using TMPro;
using UnityEngine;

namespace Main
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI characterNameTMP = default;
        [SerializeField] private TextMeshProUGUI characterDialogueTMP = default;
        [SerializeField] private TextMeshProUGUI dialogueStatusTMP = default;

        private Action onDialogueClick = default;
        private Func<bool> canProceedDialogue = default;
        public void Init(Action onDialogueClick, Func<bool> canProceedDialogue)
        {
            this.onDialogueClick = onDialogueClick;
            this.canProceedDialogue = canProceedDialogue;
        }

        public void UpdateDialoguePanel(DialogueCharacterData dialogueCharacterData)
        {
            characterDialogueTMP.text = dialogueCharacterData.text;
            characterNameTMP.text = dialogueCharacterData.character;
        }

        public void Continue()
        {
            if (!canProceedDialogue.Invoke())
                return;

            onDialogueClick?.Invoke();
        }
    }
}
