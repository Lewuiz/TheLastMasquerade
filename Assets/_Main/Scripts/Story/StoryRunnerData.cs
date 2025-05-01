using System.Collections.Generic;
using System;
using UnityEngine;

namespace Main
{
    public class StoryRunnerData
    {
        public StoryManager storyManager;
        public Action<DialogueCharacterData> updateDialoguePanel = default;
        public Action<string> updateActorConversation = default;
        public Action<DialogueActorControl> checkCharacterControl = default;
        public Action<List<string>> addCharacter = default;
        public Func<bool> isAnimating = default;
        public Action<List<DialogueEventData>> executeDialogueEvent = default;
    }
}
