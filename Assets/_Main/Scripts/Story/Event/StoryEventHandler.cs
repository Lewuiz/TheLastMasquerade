using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Main
{
    /// <summary>
    /// This class is used for executing event on dialogue.
    /// This mean will trigger something along the dialogue
    /// </summary>
    public class StoryEventHandler : MonoBehaviour
    {
        [SerializeField] private List<BackgroundEventData> backgroundEventData = new List<BackgroundEventData>();

        private Action<Sprite> changeBackground = default;
        private Action<string> loadInspectItem = default;
        private Action playTelephoneMiniGame = default;

        public void Init(Action<Sprite> changeBackground, Action<string> loadInspectItem, Action playTelephoneMiniGame)
        {
            this.changeBackground = changeBackground;
            this.loadInspectItem = loadInspectItem;
            this.playTelephoneMiniGame = playTelephoneMiniGame;
        }

        public void ExecuteEvents(List<DialogueEventData> dialoguesDataList)
        {
            if (dialoguesDataList == null)
                return;

            for (int i = 0; i < dialoguesDataList.Count; i++)
            {
                var dialogueEventData = dialoguesDataList[i];
                if (dialogueEventData.type == "change_background")
                {
                    ChangeBackground(dialogueEventData.value);
                }
                else if(dialogueEventData.type == "inspect_object")
                {
                    CreateInspectObject(dialogueEventData.value);
                }
                else if(dialogueEventData.type == "interact_puzzle")
                {
                    playTelephoneMiniGame?.Invoke();
                }
            }
        }

        private void ChangeBackground(string background)
        {
            Debug.Log(background);
            Sprite bgSprite = backgroundEventData.Find(eventData => eventData.id == background).sprite;
            
            if (bgSprite == null)
                return;
            
            changeBackground?.Invoke(bgSprite);
        }

        private void CreateInspectObject(string id)
        {
            loadInspectItem?.Invoke(id);
        }
    }
}
