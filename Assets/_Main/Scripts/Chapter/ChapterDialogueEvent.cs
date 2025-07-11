using SaintsField;
using System;
using UnityEngine;

namespace Main
{
    [Serializable]
    public class ChapterDialogueEvent
    {
        public ChapterDialogueEventPhase eventPhase;
        public ChapterDialogueEventType eventType;
        
        [ShowIf(nameof(eventType), ChapterDialogueEventType.ChangeBackground)]
        public Sprite backgroundSprite;

        [ShowIf(nameof(eventType), ChapterDialogueEventType.Audio)]
        public AudioClip audioClip;

        [ShowIf(nameof(CanShowPrefabField))]
        public GameObject prefab;

        private bool CanShowPrefabField()
        {
            return eventType == ChapterDialogueEventType.Item;
        }
    }
}
