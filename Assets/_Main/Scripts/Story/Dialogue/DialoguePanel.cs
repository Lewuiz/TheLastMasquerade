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

        private List<DialogueCharacterData> dialogueCharacterDataList = new List<DialogueCharacterData>();
        private int dialogueIdx = 0;

        private Action onDialogueEnded = default;
        private Action checkDialogueEvent = default;

        public void Init(Action onDialogueEnded, Action checkDialogueEvent)
        {
            this.onDialogueEnded = onDialogueEnded;
            this.checkDialogueEvent = checkDialogueEvent;
        }

        public void UpdateDialogueData(DialogueData dialogueData)
        {
            dialogueIdx = 0;
            dialogueCharacterDataList = dialogueData.dialogue;
        }

        public void UpdateDialoguePanel()
        {
            checkDialogueEvent?.Invoke();
            DialogueCharacterData dialogueCharacterData = dialogueCharacterDataList[dialogueIdx];
            characterDialogueTMP.text = dialogueCharacterData.text;
            characterNameTMP.text = dialogueCharacterData.character;
        }

        public void Continue()
        {
            dialogueIdx++;
            if (dialogueIdx < dialogueCharacterDataList.Count)
            {
                UpdateDialoguePanel();
            }
            else
            {
                onDialogueEnded?.Invoke();
            }
        }
    }
}
