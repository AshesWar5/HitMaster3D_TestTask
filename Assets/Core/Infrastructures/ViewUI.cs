using UnityEngine;

namespace Core.Infrastructures
{
    public class ViewUI : View
    {
        public RectTransform Rect { get; private set; }
        
        private void Awake()
        {
            Rect = GetComponent<RectTransform>();
        }

        public void UpdateVisibility(bool visibility)
        {
            if (visibility)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}