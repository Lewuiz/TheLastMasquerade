using SaintsField;
using System;
using UnityEngine;

namespace Main
{
    [Serializable]
    public class CharacterInCharge
    {
        [HideInInspector] public string name;
        public CharacterData characterData;
        public CharacterInChargeState inChargeState;
        
        [ShowIf(nameof(inChargeState), CharacterInChargeState.Show)]
        public CharacterExpression expression;

        public void Valdiate()
        {
            if (characterData != null) 
            {
                name = $"{characterData.name} | {inChargeState}";
            }
        }
    }
}
