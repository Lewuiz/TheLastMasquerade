using System.Collections.Generic;

namespace Main
{
    public class StoryData
    {
        public List<DialogueData> dialogueDataList = new List<DialogueData>();
    }

    public class DialogueData
    {
        public string dialogueId;
        public bool isAutoSave;
        public List<DialogueData> dialogueDataList = new List<DialogueData>();
        public List<DialogueChoice> dialogueChoiceList = new List<DialogueChoice>();
        public List<DialogueEvent> events = new List<DialogueEvent>();
        public List<DialogueGameMode> nextGameMode = new List<DialogueGameMode>();
    }

    public class DialogueCharacterData
    {
        public string character;
        public string text;
        public string expression;
    }

    public class DialogueChoice
    {
        public string text;
        public string nextDialogueId;
    }

    public class DialogueEvent
    {
        public string type;
        public string value;
    }

    public class DialogueGameMode
    {
        public string type;
        public string nextDialogueId;
    }
}
