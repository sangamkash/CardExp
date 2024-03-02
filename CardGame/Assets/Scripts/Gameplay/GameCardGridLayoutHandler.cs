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
        private Action onMatch;
        private LevelData levelData;

        public void CreateLayout(LevelData levelData,Action onMatch)
        {
            this.levelData = levelData;
            this.onMatch = onMatch;
            CreateLayout(levelData.gridDimension);
        }
        protected override void InitCell(GameCard obj, Vector2Int currentIndex)
        {
            var cardId = levelData.cardIds[currentIndex.x][currentIndex.y];
            obj.Init(cardData.GetCardById(cardId),cardData.BackSprite, () =>
            {
                OnCardSelected(cardId,currentIndex);
            });
        }

        private void OnCardSelected(int cardId,Vector2Int index)
        {
            if (lastSelectedCard == cardId && lastSelectedIndex != index)
            {
                GetObjByIndex(lastSelectedIndex).MarkAsReviled();
                GetObjByIndex(index).MarkAsReviled();
                onMatch?.Invoke();
            }

            lastSelectedCard = cardId;
            lastSelectedIndex = index;
        }
    }
}
