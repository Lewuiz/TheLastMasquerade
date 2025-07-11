using Papae.UnitySDK.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Main
{
    /// <summary>
    /// This class is used for executing event on dialogue.
    /// This mean will trigger something along the dialogue
    /// </summary>
    public class StoryEventHandler : MonoBehaviour
    {
        private InspectItemController inspectItemController = default;
        private Action<Sprite> changeBackground = default;

        private bool isCollectingItem = false;

        public void Init(Action<Sprite> changeBackground, InspectItemController inspectItemController)
        {
            this.changeBackground = changeBackground;
            this.inspectItemController = inspectItemController;
        }

        public void ExecuteEvents(List<ChapterDialogueEvent> chapterDialogueEventList, ChapterDialogueEventPhase phase)
        {
            if (chapterDialogueEventList == null)
                return;
            
            var chapterDialogueEventByPhase = chapterDialogueEventList.Where(dialogueEvent => dialogueEvent.eventPhase == phase);
            for (int i = 0; i < chapterDialogueEventList.Count; i++)
            {
                var chapterDialogueEvent = chapterDialogueEventList[i];
                
                if(chapterDialogueEvent.eventType == ChapterDialogueEventType.ChangeBackground)
                {
                    ChangeBackground(chapterDialogueEvent.backgroundSprite);
                }
                else if (chapterDialogueEvent.eventType == ChapterDialogueEventType.Audio)
                {
                    AudioManager.Instance.PlayOneShot(chapterDialogueEvent.audioClip);
                }
                else if (chapterDialogueEvent.eventType == ChapterDialogueEventType.Item)
                {
                    inspectItemController.Spawn(chapterDialogueEvent.prefab);
                }
                else if (chapterDialogueEvent.eventType == ChapterDialogueEventType.Battle)
                {

                }
            }
        }

        private void ChangeBackground(Sprite bgSprite)
        {
            changeBackground?.Invoke(bgSprite);
        }



        public bool IsExecutingEvent()
        {
            return false;
        }
    }
}
