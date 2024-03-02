using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardGame.GamePlay
{
    public class GameCard : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image img;
        private Sprite frontSprite;
        private Action onButtonClick;

        public void Init(Sprite frontSprite,Action onButtonClick)
        {
            this.onButtonClick = onButtonClick;
            this.frontSprite = frontSprite;
            img.sprite = frontSprite;
            img.color = new Color(1, 1, 1, 1f);
            button.interactable = transform;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClick);
            StartCoroutine(HideImage());
            if (frontSprite == null)
                MarkAsReviled();
        }

        private void OnButtonClick()
        {
            img.sprite = frontSprite;
            StopAllCoroutines();
            StartCoroutine(HideImage());
            onButtonClick?.Invoke();
        }

        private IEnumerator HideImage()
        {
            yield return new WaitForSeconds(1);
            img.sprite = null;
        }

        public void MarkAsReviled()
        {
            img.color = new Color(1, 1, 1, 0f);
            button.interactable = false;
        }
        private void OnDisable()
        {
            frontSprite = null;
        }
    }
}
