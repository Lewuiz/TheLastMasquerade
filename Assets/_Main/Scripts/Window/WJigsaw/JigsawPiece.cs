using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main
{
    public class JigsawPiece : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private SpriteRenderer jigsawPartSpriteRenderer = default;
        private PolygonCollider2D polygonCollider = default;

        private List<JigsawPieceNeighbourData> jigsawPieceNeighbourDataList = new List<JigsawPieceNeighbourData>();
        public List<JigsawPieceNeighbourData> JigsawPieceNeighbourDataList => jigsawPieceNeighbourDataList;

        private Action<JigsawPiece> checkJigsawPiece = default;
        private Func<JigsawPiece, List<JigsawPiece>> getLinkedPieces = default;

        public string Id { get; private set; } = default;

        private const int DEFAULT_SORTING_ORDER = 6;
        private const int SELECTED_JIGASW_PIECE_ORDER = 7;

        private Vector2 currentPosition = Vector2.zero;

        public void Init(Action<JigsawPiece> checkJigsawPiece, Func<JigsawPiece, List<JigsawPiece>> getLinkedPieces)
        {
            jigsawPartSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            this.checkJigsawPiece = checkJigsawPiece;
            this.getLinkedPieces = getLinkedPieces;
            Id = jigsawPartSpriteRenderer.sprite.name;

            UpdateSortingLayer(DEFAULT_SORTING_ORDER);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dragPos = new Vector3(mousePosition.x, mousePosition.y, 0f);
            Move(dragPos);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            currentPosition = transform.position;

            List<JigsawPiece> linkedPiece = getLinkedPieces.Invoke(this);
            for (int i = 0; i < linkedPiece.Count; i++)
            {
                linkedPiece[i].UpdateSortingLayer(SELECTED_JIGASW_PIECE_ORDER);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            currentPosition = Vector2.zero;
            checkJigsawPiece?.Invoke(this);

            List<JigsawPiece> linkedPiece = getLinkedPieces.Invoke(this);
            for (int i = 0; i < linkedPiece.Count; i++)
            {
                linkedPiece[i].UpdateSortingLayer(DEFAULT_SORTING_ORDER);
            }
        }

        public void CheckNeighbour()
        {
            ContactFilter2D contactFilter = new ContactFilter2D().NoFilter();

            List<Collider2D> jigsawPieceNeighbourList = new List<Collider2D>();
            Physics2D.OverlapCollider(polygonCollider, contactFilter, jigsawPieceNeighbourList);

            foreach (var neighbourJigsawPiece in jigsawPieceNeighbourList)
            {
                JigsawPiece neighbourJigsawPuzzlePiece = neighbourJigsawPiece.GetComponent<JigsawPiece>();
                Vector3 deltaPosition = new Vector3(neighbourJigsawPuzzlePiece.transform.position.x - transform.position.x, neighbourJigsawPuzzlePiece.transform.position.y - transform.position.y, 0);
                JigsawPieceNeighbourData jigsawPieceNeighbourData = new JigsawPieceNeighbourData()
                {
                    jigsawPartId = neighbourJigsawPuzzlePiece.Id,
                    deltaPosition = deltaPosition
                };

                jigsawPieceNeighbourDataList.Add(jigsawPieceNeighbourData);
            }
        }

        public void Move(Vector3 movePosition, bool isDragging = true)
        {
            if (!isDragging)
                currentPosition = transform.position;

            if (currentPosition == (Vector2)movePosition)
                return;

            List<JigsawPiece> jigsawPieces = getLinkedPieces.Invoke(this);

            Vector3 offset = movePosition - new Vector3(currentPosition.x, currentPosition.y);
            for (int i = 0; i < jigsawPieces.Count; i++)
            {
                if (jigsawPieces[i].Id == Id)
                    jigsawPieces[i].transform.position = movePosition;
                else
                    jigsawPieces[i].transform.position += offset;
            }

            currentPosition = movePosition;
        }

        public void UpdateSortingLayer(int sortingOrder)
        {
            jigsawPartSpriteRenderer.sortingOrder = sortingOrder;
        }
    }
}