using UnityEngine;

namespace Main
{
    [CreateAssetMenu(fileName = "Key Item", menuName = "Chapter/Key Item")]
    public class ChapterKeyItem : ScriptableObject
    {
        public string keyItemId;
        public string keyName;
        public Sprite keySprite;
    }
}
