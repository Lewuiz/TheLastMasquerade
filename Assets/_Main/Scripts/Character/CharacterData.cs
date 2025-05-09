using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public string characterId = default;
        public string characterName = default;
        public List<Sprite> characterSpriteList = new List<Sprite>();
    }
}
