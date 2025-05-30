using System.Collections.Generic;
using System;

namespace Main
{
    public class StoryRunnerData
    {
        public StoryManager storyManager;
        public Action<DialogueCharacterData> updateDialoguePanel = default;
        public Action<DialogueActorControl, string> onDialoguePlay = default;
        public Action<List<string>> addCharacter = default;
        public Func<bool> isAnimating = default;
        public Action<List<DialogueEventData>> executeDialogueEvent = default;
        public Action backToChapterSelectionScene = default;
        public int storyChapter = default;
        public Action hideDialogue = default;
        public Action<List<DialogueChoiceData>, Action> showDialogueChoice = default;
        public Func<bool> isShowChoice = default;
        public TelephoneController telephoneController = default;
    }
}
