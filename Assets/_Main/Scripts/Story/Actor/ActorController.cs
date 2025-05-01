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
        private readonly List<Vector2> actorPositionList = new List<Vector2>()
        {
            new Vector2(-14, -7), //position actor 1
            new Vector2(14, -7),  //position actor 2
            new Vector2(-9, -7), //position actor 3
            new Vector2(9, -7),  //position actor 4
            new Vector2(-4, -7), //position actor 5
            new Vector2(4, -7),  //position actor 6
        };

        public void Init()
        {
            for (int i = 0; i < actorPositionList.Count; i++)
            {
                Actor actor = Instantiate(actorTemplate, actorTemplate.transform.parent);
                actor.Init();
                actorList.Add(actor);
            }
        }

        public void UpdateActorConversation(string characterId)
        {

        }
    }
}
