using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.CreatorSystem
{
    public class CardLayoutHandler : MonoBehaviour
    {
        [SerializeField] private float cellSpacing=2;
        [SerializeField] private Card prefab;
        [SerializeField] private RectTransform container;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        private GenericMonoPool<Card> pool;
        private Vector2 dimension;
        private Card[][] cardLayout;
        private Card selectedCard;
        private Action<Vector2Int> onCardSelected;

        private void Awake()
        {
            pool = new GenericMonoPool<Card>(prefab);
        }

        public void CreateLayout(Vector2Int dimension,Action<Vector2Int> onCardSelected)
        {
            this.onCardSelected = onCardSelected;
            this.dimension = dimension;
            pool.ResetAllPool();
            var containerDimension = container.rect.size;
            var cellSize = Math.Min(containerDimension.x, containerDimension.y) / 
                           Math.Max(dimension.x, dimension.y) - cellSpacing;
            gridLayoutGroup.cellSize = Vector2.one * cellSize;
            gridLayoutGroup.spacing = Vector2.one * cellSpacing;
            cardLayout = new Card[dimension.x][];
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = dimension.x;
            var k = 0;
            for (var i = 0; i < dimension.x; i++)
            {
                cardLayout[i] = new Card[dimension.y];
                for (var j = 0; j < dimension.y; j++)
                {
                    var t = pool.GetObject(container);
                    var ci = new Vector2Int(i, j);
                    t.Init(() =>
                    {
                        OnCardSelected(ci);
                    }, null, false);
                    cardLayout[i][j] = t;
                    if (i == 0 && j == 0)
                    {
                        selectedCard = t;
                        t.MarkAsSelected(true);
                    }
                    else
                    {
                        t.MarkAsSelected(false);
                    }
                    t.transform.SetSiblingIndex(k);
                    k++;
                }
            }
        }

        private Card GetCardAtIndex(Vector2Int index)
        {
            if (index.x < dimension.x && index.y < dimension.y && index.x>=0 && index.y >=0)
            {
                return cardLayout[index.x][index.y];
            }
            return null;
        }

        public void SetImageAtIndex(Vector2Int index, Sprite sprite)
        {
            var card = GetCardAtIndex(index);
            if (card != null)
            {
                card.SetImage(sprite);
            }
        }

        private void OnCardSelected(Vector2Int index)
        {
            for (var i = 0; i < dimension.x; i++)
            {
                for (var j = 0; j < dimension.y; j++)
                {
                    var t=  cardLayout[i][j];
                    if (i == index.x && j == index.y)
                    {
                        selectedCard = t;
                        t.MarkAsSelected(true);
                    }
                    else
                    {
                        t.MarkAsSelected(false);
                    }
                }
            }
            onCardSelected?.Invoke(index);
        }

    }
}