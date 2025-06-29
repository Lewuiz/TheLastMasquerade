using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public string characterId = default;
        public string characterName = default;
        public List<CharacterState> characterSpriteList = new List<CharacterState>();
    }

    [Serializable]
    public class CharacterState
    {
        public Sprite sprite;
        public CharacterExpression expression;
    }
}
