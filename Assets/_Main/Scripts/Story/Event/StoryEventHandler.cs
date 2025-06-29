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
        private Action<Sprite> changeBackground = default;

        public bool IsExecuting { get; private set; } = false;

        public void Init(Action<Sprite> changeBackground)
        {
            this.changeBackground = changeBackground;
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

                }
                else if (chapterDialogueEvent.eventType == ChapterDialogueEventType.Item)
                {

                }
                else if (chapterDialogueEvent.eventType == ChapterDialogueEventType.Battle)
                {

                }
                else if (chapterDialogueEvent.eventType == ChapterDialogueEventType.MiniGame)
                {

                }
            }
        }

        private void ChangeBackground(Sprite bgSprite)
        {
            changeBackground?.Invoke(bgSprite);
        }

        private void CreateInspectObject(string id)
        {

        }
    }
}
