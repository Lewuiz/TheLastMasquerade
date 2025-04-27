using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class ActorController : MonoBehaviour
    {
        [SerializeField] private List<CharacterData> characterDataList = new List<CharacterData>();
        [SerializeField] private Actor actorTemplate = default;

        private List<Actor> actorList = new List<Actor>();
        private readonly List<Vector2> actorPosition = new List<Vector2>()
        {
            new Vector2(-10, -7), //position actor 1
            new Vector2(10, -7),  //position actor 2
            new Vector2(-5, -7), //position actor 3
            new Vector2(5, -7),  //position actor 4
        };

        public void Init()
        {

        }

        public void UpdateActorConversation(string characterId)
        {

        }
    }
}
