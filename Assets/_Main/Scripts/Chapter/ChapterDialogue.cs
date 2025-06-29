using SaintsField;
using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Main
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Chapter/Dialogue")]
    public class ChapterDialogue : ScriptableObject
    {
        public string dialogueId;
        public List<CharacterDialogue> dialogueList = new List<CharacterDialogue>();
        public List<DialogueChoiceData> dialogueChoiceData = new List<DialogueChoiceData>();
        public string requiredToProceedNextDialogue = default;
        [ShowIf(nameof(CanNextDialogueField))] public string nextDialogueId;

        private bool CanNextDialogueField()
        {
            return dialogueChoiceData.Count == 0;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < dialogueList.Count; i++)
            {
                dialogueList[i].Validate();
            }
            EditorUtility.SetDirty(this);
        }
#endif
    }

    [Serializable]
    public class CharacterDialogue
    {
        public string text;
        public CharacterData characterData;
        public List<CharacterInCharge> characterInChargeList = new List<CharacterInCharge>();
        public List<ChapterDialogueEvent> chapterDialogueEventList = new List<ChapterDialogueEvent>();

        public void Validate()
        {
            for (int i = 0; i < characterInChargeList.Count; i++)
            {
                characterInChargeList[i].Valdiate();
            }
        }
    }
}
