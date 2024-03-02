using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace CardGame
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Image img;
        [SerializeField] private Button button;
        [SerializeField] private Image selectImg;
        [SerializeField] private Color defaultColor;
        private Action onClickCallBack;

        public void Init(Action onClickCallBack,Sprite sprite,bool markAsSelected)
        {
            this.onClickCallBack = onClickCallBack;
            button.onClick.AddListener(OnButtonClick);
            SetImage(sprite);
            MarkAsSelected(markAsSelected);
        }
        
        private void OnButtonClick()
        {
            onClickCallBack?.Invoke();
        }

        public void SetImage(Sprite sprite)
        {
            img.sprite = sprite;
            if (sprite == null)
                img.color = defaultColor;
            else
                img.color=Color.white;
        }

        public void MarkAsSelected(bool isSelected)
        {
            var t = selectImg.color;
            selectImg.color = isSelected ? new Color(t.r, t.g, t.b, 1) : new Color(t.r, t.g, t.b, 0);
        }
    }
}