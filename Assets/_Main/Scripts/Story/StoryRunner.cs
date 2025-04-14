using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class StoryRunner : MonoBehaviour
    {
        [SerializeField] private List<CharacterData> characterDataList = new List<CharacterData>();
        [SerializeField] private SpriteRenderer backgroundSR = default;
        [SerializeField] private BackgroundSizeFitter backgroundSizeFitter = default;
        [SerializeField] private DialoguePanel dialoguePanel = default;
        [SerializeField] private DialogueChoicePanel dialogueChoicePanel = default;

        public void Init() 
        {
            dialoguePanel.Init(PlayNextDialogue);
            dialogueChoicePanel.Init();
        }

        private void ShowDialogueChoice()
        {
            dialogueChoicePanel.gameObject.SetActive(true);
            dialogueChoicePanel.ShowDialogue();
        }

        public void PlayNextDialogue()
        {

        }

        private void UpdateBackground(Sprite sprite)
        {
            backgroundSR.sprite = sprite;
            backgroundSizeFitter.FitToCamera();
        }
    }
}
