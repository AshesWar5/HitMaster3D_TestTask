using Core.Infrastructures;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Logic
{
    public class ImageView : ViewUI
    {
        [SerializeField] private Image _image;

        public Material Material => _image.material;

        public void SetSprite(Sprite sprite) =>
            _image.sprite = sprite;
        
        public void SetColor(Color color) =>
            _image.color = color;

        public void SetFillAmount(float value) =>
            _image.fillAmount = value;
    }
}