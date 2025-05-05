using System.Collections.Generic;
using Core.Services.Page;
using UnityEngine;

namespace Core.Infrastructures.Factories.UI
{
    public interface IUIPageFactory : IRemoveView
    {
        IReadOnlyDictionary<PageType, PageView> Pages { get; }
        
        TypePage Create<TypePage>(PageType type, 
            Vector3 pos = new Vector3(), Transform parent = null) where TypePage : PageView;
    }
}