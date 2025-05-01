using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public void Init(Action onDialogueClick)
        {
            this.onDialogueClick = onDialogueClick;
        }

        public void UpdateDialoguePanel(DialogueCharacterData dialogueCharacterData)
        {
            characterDialogueTMP.text = dialogueCharacterData.text;
            characterNameTMP.text = dialogueCharacterData.character;
        }

        public void Continue()
        {
            onDialogueClick?.Invoke();
        }
    }
}
