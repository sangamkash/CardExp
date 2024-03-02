using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.GameData;
using UnityEngine;

namespace CardGame.GamePlay
{
    public class GameCardGridLayoutHandler : CardLayoutHandler<GameCard>
    {
        [SerializeField] private CardData cardData;
        private int lastSelectedCard = -1;
        private Vector2Int lastSelectedIndex= new Vector2Int(-1,-1);
        private Action<bool> onMatch;
        private LevelData levelData;
        private int clickCount;
        private int[][] gridData;

        public void CreateLayout(LevelData levelData,Action<bool> onMatch,int[][] gridData)
        {
            this.gridData = gridData;
            this.levelData = levelData;
            this.onMatch = onMatch;
            CreateLayout(levelData.gridDimension);
        }
        protected override void InitCell(GameCard obj, Vector2Int currentIndex)
        {
            var cardId = levelData.cardIds[currentIndex.x][currentIndex.y];
            obj.Init(cardData.GetCardById(cardId), () =>
            {
                OnCardSelected(cardId,currentIndex);
            });
        }

        private void OnCardSelected(int cardId,Vector2Int index)
        {
            clickCount++;
            var matchFound = false;
            if (lastSelectedCard == cardId && lastSelectedIndex != index)
            {
                GetObjByIndex(lastSelectedIndex).MarkAsReviled();
                GetObjByIndex(index).MarkAsReviled();
                gridData[lastSelectedIndex.x][lastSelectedIndex.y] = -1;
                gridData[index.x][index.y] = -1;
                matchFound = true;
                clickCount = 0;
            }
            lastSelectedCard = cardId;
            lastSelectedIndex = index;
            if (clickCount%2 ==0)
            {
                onMatch?.Invoke(matchFound);
            }
        }
    }
}
