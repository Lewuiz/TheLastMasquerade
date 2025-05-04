using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main
{
    public class InspectItemController : MonoBehaviour
    {
        [SerializeField] private List<InspectItemGameData> inspectItemGameDataList = new List<InspectItemGameData>();
        [SerializeField] ObtaineditemPanel obtainedItemPanel = default;

        private GameObject inspectGame = default;
        private List<Inspectitem> inspecItemList = new List<Inspectitem>();

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
            inspecItemList = FindObjectsByType<Inspectitem>(FindObjectsSortMode.None).ToList();

            for (int i = 0; i < inspecItemList.Count; i++)
            {
                inspecItemList[i].Init(OnItemFound);
            }
        }

        private void OnItemFound(Inspectitem inspectItem)
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
