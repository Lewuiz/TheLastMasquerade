using System.Collections.Generic;

namespace Main
{
    public class StoryData
    {
        public int chapter = default;
        public List<DialogueData> dialogueDataList = new List<DialogueData>();
    }

    public class DialogueData
    {
        public string dialogueId;
        public bool isAutoSave;
        public List<DialogueCharacterData> dialogue = new List<DialogueCharacterData>();
        public List<DialogueEventData> events = new List<DialogueEventData>();
        //public List<DialogueChoiceData> choices = new List<DialogueChoiceData>();
        public DialogueGameModeData nextGameMode = new DialogueGameModeData();
    }

    public class DialogueCharacterData
    {
        public string character;
        public string text;
        public string expression;
    }

    public class DialogueChoiceData
    {
        public string text;
        public string nextDialogueId;
    }

    public class DialogueEventData
    {
        public string type;
        public string value;
    }

    public class DialogueGameModeData
    {
        public string type;
        public string nextDialogueId;
    }
}
