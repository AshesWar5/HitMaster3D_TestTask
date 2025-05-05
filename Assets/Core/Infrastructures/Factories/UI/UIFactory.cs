using System;
using System.Collections.Generic;
using Core.Infrastructures.AssetManagement;
using Core.Services.Page;
using UnityEngine;

namespace Core.Infrastructures.Factories.UI
{
    public sealed class UIFactory : IUIPageFactory, IUIElementFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<PageType, PageView> _pages;

        public IReadOnlyDictionary<PageType, PageView> Pages => _pages;

        public UIFactory(IAssetProvider asset)
        {
            _assetProvider = asset;
            _pages = new Dictionary<PageType, PageView>();
        }

        TypePage IUIPageFactory.Create<TypePage>(PageType type, Vector3 pos = new Vector3(),
            Transform parent = null)
        {
            _pages.TryGetValue(type, out PageView page);
            if (page is not null)
            {
                if(!page.IsInitialized)
                    page.Initialize();
                
                return page as TypePage;
            }
            
            var instance = _assetProvider.Instantiate<TypePage>(GetPathPage(type), parent, pos, Vector3.one);
            instance.Initialize();
            _pages.Add(type, instance);
            return instance;
        }

        TTypeElement IUIElementFactory.Create<TTypeElement>(UIElementType type, Vector3 pos = new Vector3(),
            Transform parent = null)
        {
            TTypeElement instance = _assetProvider.Instantiate<TTypeElement>(GetPathElement(type),
                parent, pos, Vector3.one);
            instance.Initialize();
            return instance;
        }

        public void Remove(View view)
        {
            try
            {
                if (view is not null)
                {
                    if (view is PageView page)
                    {
                        _pages.Remove(page.Type);
                    }
                    
                    view.Dispose();
                    _assetProvider.Destroy(view.gameObject);
                }
            }
            catch { }
        }

        private string GetPathPage(PageType type)
        {
            return type switch
            {
                PageType.MobileInput => AssetPath.MOBILE_INPUT_PAGE,
                PageType.Game => AssetPath.GAME_PAGE,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        private string GetPathElement(UIElementType type)
        {
            return type switch
            {
                UIElementType.HealthEnemy => AssetPath.HEALTH_ENEMY_UI_ELEMENT,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}