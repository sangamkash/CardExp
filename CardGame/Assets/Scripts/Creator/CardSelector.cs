using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.GameData;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.CreatorSystem
{
    public class CardSelector : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private CardData cardData;
        [SerializeField] private Button btnPrefab;
        private Action<int> onCardSelectedCallBack;
        private void Start()
        {
            for (int i = 0; i < cardData.GetCardsCount; i++)
            {
                var index = i;
                var sprite = cardData.GetCardById(i);
                var t = Instantiate(btnPrefab.gameObject, container).GetComponent<Button>();
                t.onClick.AddListener(() => OnCardSelect(index));
                t.GetComponent<Image>().sprite = sprite;
            }
        }

        private void OnCardSelect(int id)
        {
            onCardSelectedCallBack?.Invoke(id);
        }

        public void Init(Action<int> onCardSelected)
        {
            this.onCardSelectedCallBack = onCardSelected;
        }
    }
}
