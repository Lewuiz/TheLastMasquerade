using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class DialogueChoice : MonoBehaviour
    {
        [SerializeField] private Image dialogueButtonBackground = default;
        [SerializeField] private TextMeshProUGUI dialogueChoiceTMP = default;

        public string NextDialogueId { get; private set; } = default;
        private Action<DialogueChoice> onDialogueSelect = default;

        public void Init(Action<DialogueChoice> onDialogueSelect)
        {
            this.onDialogueSelect = onDialogueSelect;
        }

        public void UpdateDialogue(string nextDialogueId, string dialogueChoice)
        {
            NextDialogueId = nextDialogueId;
            dialogueChoiceTMP.text = dialogueChoice;
        }

        public void OnDialogueSelect()
        {
            onDialogueSelect?.Invoke(this);
        }

        public void UpdateDialogueButton(Color32 color)
        {
            dialogueButtonBackground.color = color;
        }
    }
}
