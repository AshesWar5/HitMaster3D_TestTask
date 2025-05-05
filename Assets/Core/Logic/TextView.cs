using Core.Infrastructures;
using TMPro;
using UnityEngine;

namespace Core.Logic
{
    public class TextView : ViewUI
    {
        [SerializeField] private TMP_Text _text;

        public string Text => _text.text;

        public void SetText(string text) => _text.text = text;
        
        public void SetColor(Color color) => _text.color = color;
    }
}