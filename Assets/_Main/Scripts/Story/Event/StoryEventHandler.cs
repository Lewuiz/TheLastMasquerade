using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    /// <summary>
    /// This class is used for executing event on dialogue.
    /// This mean will trigger something along the dialogue
    /// </summary>
    public class StoryEventHandler : MonoBehaviour
    {
        [SerializeField] private List<BackgroundEventData> backgroundEventData = new List<BackgroundEventData>();

        private Action<Sprite> changeBackground = default;
        

        public void Init(Action<Sprite> changeBackground)
        {
            this.changeBackground = changeBackground;
        }

        public void ExecuteEvents(List<DialogueEventData> dialoguesDataList)
        {
            if (dialoguesDataList == null)
                return;

            for (int i = 0; i < dialoguesDataList.Count; i++)
            {
                var dialogueEventData = dialoguesDataList[i];
                if (dialogueEventData.type == "change_background")
                {
                    ChangeBackground(dialogueEventData.value);
                }
            }
        }

        private void ChangeBackground(string background)
        {
            Sprite bgSprite = backgroundEventData.Find(eventData => eventData.id == background).sprite;
            
            if (bgSprite == null)
                return;
            
            changeBackground?.Invoke(bgSprite);
        }
    }
}
