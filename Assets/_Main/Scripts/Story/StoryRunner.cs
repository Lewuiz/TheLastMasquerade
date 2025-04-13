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
        [SerializeField] private DialogueChoice dialogueChoice = default;

        public void Init() 
        {
            dialoguePanel.Init(PlayNextDialogue);
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
