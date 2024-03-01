using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.GameData
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardGame/CardData", order = 1)]
    public class CardData : ScriptableObject
    {
        [SerializeField] private Sprite[] cards;
        public int GetCardsCount => cards.Length;
        public Sprite GetCardById(int id) => cards[id];
    }
}