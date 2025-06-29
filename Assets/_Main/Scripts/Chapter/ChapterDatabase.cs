using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(fileName = "Chapter Database", menuName = "Chapter/Database")]
    public class ChapterDatabase : ScriptableObject
    {
        public List<ChapterData> chapters = new List<ChapterData>();
    }
}
