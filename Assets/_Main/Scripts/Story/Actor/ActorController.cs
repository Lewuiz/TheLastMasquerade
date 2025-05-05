using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class ActorController : MonoBehaviour
    {
        [SerializeField] private List<CharacterData> characterDataList = new List<CharacterData>();
        [SerializeField] private Actor actorTemplate = default;

        private List<Actor> actorList = new List<Actor>();
        private List<Actor> inDialogueActorList = new List<Actor>();

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
            actorTemplate.gameObject.SetActive(false);
            for (int i = 0; i < actorPositionList.Count; i++)
            {
                Actor actor = Instantiate(actorTemplate, actorTemplate.transform.parent);
                actor.Init();
                actorList.Add(actor);
            }
        }

        public void HideAllCharacter()
        {
            for (int i = 0; i < inDialogueActorList.Count; i++)
            {
                inDialogueActorList[i].Hide();
            }
        }

        public void ShowAllCharacter()
        {
            for (int i = 0; i < inDialogueActorList.Count; i++)
            {
                inDialogueActorList[i].Show();
            }
        }

        public void AddCharacterInDialogue(List<string> actorsData)
        {
            StartCoroutine(AddCharacterInDialogueCor(actorsData));
        }

        public IEnumerator AddCharacterInDialogueCor(List<string> actorsData)
        {
            if (actorsData == null)
                yield break;

            while (IsAnimatingActor())
                yield return null;

            for (int i = 0; i < actorsData.Count; i++)
            {
                string actorData = actorsData[i];

                string[] splits = actorData.Split(":");
                bool isAutoPlacing = splits.Length == 1;
                Vector3 position = isAutoPlacing ? actorPositionList[i] : actorPositionList[int.Parse(splits[1])];
                string actorId = isAutoPlacing ? actorData : splits[0];

                Actor inDialogueActor = inDialogueActorList.Find(inDialogueCharacter => inDialogueCharacter.CharacterData.characterId == actorId);
                if (inDialogueActor != null)
                    continue;

                CharacterData characterData = characterDataList.Find(character => character.characterId == actorId);

                Actor actor = actorList.Find(x => !x.IsInDialogue);
                inDialogueActorList.Add(actor);

                actor.transform.localPosition = position;
                actor.SetCharacterData(characterData);
                actor.Show();
                actor.SetActorInDialogue(true);
            }
        }

        public void RemoveActorInDialogue(List<string> characterIdList)
        {
            StartCoroutine(RemoveActorInDialogueCor(characterIdList));
        }

        public IEnumerator RemoveActorInDialogueCor(List<string> characterIdList)
        {
            if (characterIdList == null)
                yield break;

            while (IsAnimatingActor())
                yield return null;

            for (int i = 0; i < characterIdList.Count; i++)
            {
                var actor = inDialogueActorList.Find(dialogueCharacter => dialogueCharacter.CharacterData.characterId == characterIdList[i]);
                if (actor == null)
                    continue;
                
                actor.Hide();
                inDialogueActorList.Remove(actor);
            }
        }

        public void UpdateActorConversation(string characterId)
        {
            StartCoroutine(UpdateActorConversationCor(characterId));
        }

        public IEnumerator UpdateActorConversationCor(string characterId)
        {
            while (IsAnimatingActor())
                yield return null;

            for (int i = 0; i < inDialogueActorList.Count; i++)
            {
                Actor actor = inDialogueActorList[i];

                if (actor.CharacterData.characterId == characterId)
                {
                    actor.ShowConversation();
                }
                else
                {
                    actor.HideConversation();
                }
            }
        }

        public bool IsAnimatingActor()
        {
            for (int i = 0; i < inDialogueActorList.Count; i++)
            {
                if (inDialogueActorList[i].IsPlayingAnimation)
                    return true;
            }
            return false;
        }
    }
}
