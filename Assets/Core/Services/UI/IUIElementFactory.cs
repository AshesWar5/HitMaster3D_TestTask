using Core.Infrastructures;
using UnityEngine;

namespace Core.Services.UI
{
    public interface IUIElementFactory : IRemoveView
    {
        TTypeElement Create<TTypeElement>(UIElementType type, 
            Vector3 pos = new Vector3(), Transform parent = null) where TTypeElement : ViewUI;
    }
}