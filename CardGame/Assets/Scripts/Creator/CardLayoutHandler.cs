using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.CreatorSystem
{
    public class CardLayoutHandler : CardLayoutHandler<Card>
    {
        private Card selectedCard;
        private Action<Vector2Int> onCardSelected;
        

        public void CreateLayout(Vector2Int dimension,Action<Vector2Int> onCardSelected)
        {
            this.onCardSelected = onCardSelected;
            CreateLayout(dimension);
        }

        protected override void InitCell(Card obj, Vector2Int currentIndex)
        {
            obj.Init(() =>
            {
                OnCardSelected(currentIndex);
            }, null, false);
            if (currentIndex.x == 0 && currentIndex.y == 0)
            {
                selectedCard = obj;
                obj.MarkAsSelected(true);
            }
            else
            {
                obj.MarkAsSelected(false);
            }
        }

        public void SetImageAtIndex(Vector2Int index, Sprite sprite)
        {
            var card = GetObjByIndex(index);
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
                    var t=  objLayout[i][j];
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