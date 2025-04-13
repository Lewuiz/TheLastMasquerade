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

        private Action playNextDialogue = default;

        public void Init(Action playNextDialogue)
        {
            this.playNextDialogue = playNextDialogue;
        }

        public void UpdateDialogue(DialogueCharacterData dialogueCharacterData)
        {
            characterDialogueTMP.text = dialogueCharacterData.text;
            characterNameTMP.text = dialogueCharacterData.character;
        }

        public void Continue()
        {
            playNextDialogue?.Invoke();
        }
    }
}
