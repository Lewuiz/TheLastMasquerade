using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class StoryRunner : MonoBehaviour
    {
        [SerializeField] private List<CharacterData> characterDataList = new List<CharacterData>();
        [SerializeField] private SpriteRenderer backgroundSR = default;
        [SerializeField] private DialoguePanel dialoguePanel = default;

        public void Init() 
        {
            
        }

        public void PlayNextDialogue()
        {

        }

        private void UpdateBackground()
        {

        }
    }
}
