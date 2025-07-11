using System;

namespace Main
{
    public class StoryRunnerData
    {
        public StoryManager storyManager = default;
        public ActorController actorController = default;
        public DialogueChoicePanel dialogueChoicePanel = default;
        public DialoguePanel dialoguePanel = default;
        public StoryEventHandler storyEventHandler = default;
        public InspectItemController inspectItemController = default;
        public Action backToChapterSelectionScene = default;
        public Action backToTitleScene = default;
    }
}
