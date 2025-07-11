using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class JigsawController : MonoBehaviour 
    {
        [SerializeField] private List<JigsawPiece> jigsawPuzzlePieces = new List<JigsawPiece>();
        [SerializeField] private Transform jigsawParent = default;
        private List<List<JigsawPiece>> groupJigsawPiecesList = new List<List<JigsawPiece>>();

        public const float SNAP_TOLERANCE = .8f;
        private Action onJigsawSolved = default;

        public void Init()
        {
            InitJigsaw();
            CheckJigsawPieceNeighbour();
            ShufflePieces();
        }

        public void SetOnCompleted(Action onJigsawSolved)
        {
            this.onJigsawSolved = onJigsawSolved;
        }

        public void Show()
        {
            jigsawParent.gameObject.SetActive(true);
        }

        public void Hide()
        {
            jigsawParent.gameObject.SetActive(false);
        }

        private void CheckJigsawPieceNeighbour()
        {
            for (int i = 0; i < jigsawPuzzlePieces.Count; i++)
            {
                jigsawPuzzlePieces[i].CheckNeighbour();
            }
        }

        private void InitJigsaw()
        {
            for (int i = 0; i < jigsawPuzzlePieces.Count; i++)
            {
                JigsawPiece jigsawPuzzlePart = jigsawPuzzlePieces[i];
                jigsawPuzzlePart.Init(CheckJigsawPuzzleGame, GetGroupJigsawPiece);
            }
        }

        private void CheckJigsawPuzzleGame(JigsawPiece jigsawPiece)
        {
            List<JigsawPiece> linkedPieces = GetGroupJigsawPiece(jigsawPiece);
            TryToGroupJigsawPiece(linkedPieces);

            if (HasGameCompleted())
            {
                onJigsawSolved?.Invoke();
                Hide();
            }
        }

        /// <summary>
        /// If the piece's surrondings have its neighbour, then we have to group it
        /// </summary>
        /// <param name="groupPieces"></param>
        private void TryToGroupJigsawPiece(List<JigsawPiece> groupPieces)
        {
            for (int i = 0; i < groupPieces.Count; i++)
            {
                for (int j = 0; j < jigsawPuzzlePieces.Count; j++)
                {
                    JigsawPieceNeighbourData pieceNeighbourData = jigsawPuzzlePieces[j].JigsawPieceNeighbourDataList.Find(neighbourData => neighbourData.jigsawPartId == groupPieces[i].Id);

                    if (pieceNeighbourData == null)
                        continue;

                    if (CanSnap(groupPieces[i].transform, jigsawPuzzlePieces[j].transform, pieceNeighbourData.deltaPosition))
                    {
                        Vector3 movePosition = new Vector3(jigsawPuzzlePieces[j].transform.position.x, jigsawPuzzlePieces[j].transform.position.y, 0f) + pieceNeighbourData.deltaPosition;
                        groupPieces[i].Move(movePosition, false);
                        MergeGroupJigsaw(groupPieces[i], jigsawPuzzlePieces[j]);
                    }
                }
            }
        }

        private bool CanSnap(Transform selectedJigsaw, Transform targetedJigsaw, Vector3 offset)
        {
            Vector3 position = targetedJigsaw.position + offset;
            return Vector2.Distance(selectedJigsaw.position, position) <= SNAP_TOLERANCE;
        }

        private List<JigsawPiece> GetGroupJigsawPiece(JigsawPiece jigsawPiece)
        {
            for (int i = 0; i < groupJigsawPiecesList.Count; i++)
            {
                JigsawPiece jigsawPieces = groupJigsawPiecesList[i].Find(pieces => pieces.Id == jigsawPiece.Id);

                if (jigsawPieces != null)
                    return groupJigsawPiecesList[i];
            }

            return new List<JigsawPiece>() { jigsawPiece };
        }

        private void MergeGroupJigsaw(JigsawPiece selectedPiece, JigsawPiece targetedPiece)
        {
            List<JigsawPiece> selectedGroupPieces = GetGroupJigsawPiece(selectedPiece);
            List<JigsawPiece> targetedGroupPieces = GetGroupJigsawPiece(targetedPiece);

            groupJigsawPiecesList.Remove(selectedGroupPieces);
            groupJigsawPiecesList.Remove(targetedGroupPieces);

            for (int i = 0; i < targetedGroupPieces.Count; i++)
            {
                if (!selectedGroupPieces.Contains(targetedGroupPieces[i]))
                    selectedGroupPieces.Add(targetedGroupPieces[i]);
            }

            groupJigsawPiecesList.Add(selectedGroupPieces);
        }

        private bool HasGameCompleted()
        {
            return groupJigsawPiecesList.Count == 1 && groupJigsawPiecesList[0].Count == jigsawPuzzlePieces.Count;
        }

        private void ShufflePieces()
        {
            Vector3 bounds = new Vector3(.2f, .9f);
            for (int i = 0; i < jigsawPuzzlePieces.Count; i++)
            {
                float xPos = UnityEngine.Random.Range(bounds.x, bounds.y);
                float yPos = UnityEngine.Random.Range(bounds.x, bounds.y);

                Vector3 spawnPosition = new Vector3(xPos, yPos, 0);
                spawnPosition = Camera.main.ViewportToWorldPoint(spawnPosition);
                spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0f);

                jigsawPuzzlePieces[i].transform.position = spawnPosition;
            }
        }
    }
}