using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main
{
    public class InspectItemController : MonoBehaviour
    {
        [SerializeField] private List<InspectItemGameData> inspectItemGameDataList = new List<InspectItemGameData>();
        [SerializeField] ObtainedItemPanel obtainedItemPanel = default;

        private GameObject inspectGame = default;
        private List<InspectItem> inspecItemList = new List<InspectItem>();

        public bool IsGameOnGoing { get; private set; } = false;

        public void Init()
        {
            //will implement with inventory manager.
        }

        public void Load(string gameId)
        {
            IsGameOnGoing = true;
            InspectItemGameData inspectItemGameData = inspectItemGameDataList.Find(gameData => gameData.gameId == gameId);

            if (inspectItemGameData == null)
            {
                IsGameOnGoing = false;
                return;
            }

            inspectGame = Instantiate(inspectItemGameData.inspectItemGame, transform.parent);
            inspectGame.transform.position = Vector3.zero;
            inspectGame.transform.localScale = Vector3.one;

            IntializeInspectItem();
        }

        public void ShowObtainedPanel()
        {
            obtainedItemPanel.gameObject.SetActive(true);
            obtainedItemPanel.Show(null);
        }

        private void IntializeInspectItem()
        {
            inspecItemList = FindObjectsByType<InspectItem>(FindObjectsSortMode.None).ToList();

            for (int i = 0; i < inspecItemList.Count; i++)
            {
                inspecItemList[i].Init(OnItemFound);
            }
        }

        private void OnItemFound(InspectItem inspectItem)
        {

        }
    }

    [Serializable]
    public class InspectItemGameData
    {
        public string gameId;
        public GameObject inspectItemGame = default;
    }
}
