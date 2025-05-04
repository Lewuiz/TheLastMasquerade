using System;
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
        public List<string> defaultActors;
        public List<DialogueCharacterData> dialogue;
        public List<DialogueChoiceData> choices;
        public string nextDialogueId;
    }

    public class DialogueCharacterData
    {
        public string character;
        public string text;
        public string expression;
        public List<DialogueEventData> events;
        public DialogueActorControl actorControl;
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

    public class DialogueActorControl
    {
        public List<string> hide;
        public List<string> show;
    }
}
