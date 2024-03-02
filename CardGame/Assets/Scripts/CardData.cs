using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.GameData
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardGame/CardData", order = 1)]
    public class CardData : ScriptableObject
    {
        [SerializeField] private Sprite[] cards;
        [SerializeField] private Sprite backSprite;
        public int GetCardsCount => cards.Length;
        public Sprite BackSprite => backSprite;
        public Sprite GetCardById(int id)
        {
            if(id>=0 && id<cards.Length)
            return cards[id];
            return null;
        }
    }
}