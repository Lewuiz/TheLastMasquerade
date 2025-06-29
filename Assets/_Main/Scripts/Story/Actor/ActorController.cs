using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main
{
    public class ActorController : MonoBehaviour
    {
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

        private int totalActorInDialogue = 0;
        public bool IsAnimating = false;

        public void Init()
        {
            SetDefault();

            actorTemplate.gameObject.SetActive(false);
            for (int i = 0; i < actorPositionList.Count; i++)
            {
                Actor actor = Instantiate(actorTemplate, actorTemplate.transform.parent);
                actor.Init();
                actorList.Add(actor);
            }
        }

        private void SetDefault()
        {
            totalActorInDialogue = 0;
        }

        private Actor GetAvailableActor()
        {
            for (int i = 0; i < actorList.Count; i++)
            {
                if (actorList[i].CharacterData == null)
                    return actorList[i];
            }
            return null;
        }

        private Actor GetActorOnDialogue(string characterId)
        {
            for (int i = 0; i < actorList.Count; i++)
            {
                Actor actor = actorList[i];
                if (actor.CharacterData != null && actorList[i].CharacterData.characterId == characterId)
                    return actor;
            }
            return null;
        }

        private IEnumerator CheckDialogueCharacter(List<CharacterInCharge> characterInChargeList)
        {
            var sortedCharacterInChargeList = characterInChargeList.OrderBy(c => c.inChargeState).ToList();

            for (int i = 0; i < sortedCharacterInChargeList.Count; i++)
            {
                CharacterInCharge characterInCharge = characterInChargeList[i];
                if (characterInCharge.inChargeState == CharacterInChargeState.Hide)
                {
                    Actor actor = GetActorOnDialogue(characterInCharge.characterData.characterId);
                    if (actor == null)
                    {
                        Debug.LogWarning($"[ActorController] There isn't any spesific character id: {characterInCharge.characterData.characterId} on dialogue, proceed to ignore");
                        continue;
                    }

                    actor.Hide();
                    totalActorInDialogue--;
                }
                else if (characterInCharge.inChargeState == CharacterInChargeState.Show)
                {
                    if (IsActorOnDialogue(characterInCharge.characterData.characterId))
                        continue;

                    Actor availableActor = GetAvailableActor();
                    if (availableActor == null)
                    {
                        Debug.LogError($"[ActorController] There isn't any available actor");
                        continue;
                    }

                    availableActor.UpdateCharacter(characterInCharge);
                    availableActor.SetLocalPosition(actorPositionList[totalActorInDialogue]);
                    availableActor.Show();
                    totalActorInDialogue++;
                }

                while (IsAnimatingActor())
                    yield return null;
            }
        }

        public IEnumerator UpdateActorsConversationCor(CharacterDialogue characterDialogue)
        {
            IsAnimating = true;
            yield return CheckDialogueCharacter(characterDialogue.characterInChargeList);

            for (int i = 0; i < actorList.Count; i++)
            {
                Actor actor = actorList[i];

                if (actor.CharacterData == null || characterDialogue.characterData == null)
                {
                    actor.HideConversation();
                    continue;
                }

                if (actor.CharacterData.characterId == characterDialogue.characterData.characterId)
                    actor.ShowConversation();
                else
                    actor.HideConversation();
            }

            while (IsAnimatingActor())
                yield return null;

            IsAnimating = false;
        }

        public void UpdateActorsConversation(CharacterDialogue characterDialogue)
        {
            StartCoroutine(UpdateActorsConversationCor(characterDialogue));
        }

        private bool IsActorOnDialogue(string characterId)
        {
            for (int i = 0; i < actorList.Count; i++)
            {
                Actor actor = actorList[i];
                if (actor.CharacterData == null)
                    continue;

                if (actor.CharacterData.characterId == characterId)
                    return true;
            }
            return false;
        }

        private bool IsAnimatingActor()
        {
            for (int i = 0; i < actorList.Count; i++)
            {
                if (actorList[i].IsPlayingAnimation)
                    return true;
            }
            return false;
        }
    }
}
