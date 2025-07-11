using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [Serializable]
    public class ChapterData
    {
        public string chapterId = default;
        public List<ChapterDialogue> dialogueList = new List<ChapterDialogue>();
        public Sprite thumbnail = default;
    }
}
