using System;
using System.Collections.Generic;
using Core.Infrastructures;
using Core.Infrastructures.AssetManagement;
using Core.Services.Page;
using UnityEngine;

namespace Core.Services.UI
{
    public sealed class UIFactory : IUIPageFactory, IUIElementFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IAssetInstantiator _assetInstantiator;
        private readonly Dictionary<PageType, PageView> _pages;

        public IReadOnlyDictionary<PageType, PageView> Pages => _pages;

        public UIFactory(IAssetProvider assetProvider, IAssetInstantiator assetInstantiator)
        {
            _assetProvider = assetProvider;
            _assetInstantiator = assetInstantiator;
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
            
            var prefab = _assetProvider.Load<TypePage>(GetPathPage(type));
            var instance = _assetInstantiator.Instantiate(prefab, parent, pos, Vector3.one);
            instance.Initialize();
            _pages.Add(type, instance);
            return instance;
        }

        TTypeElement IUIElementFactory.Create<TTypeElement>(UIElementType type, Vector3 pos = new Vector3(),
            Transform parent = null)
        {
            var prefab = _assetProvider.Load<TTypeElement>(GetPathElement(type));
            TTypeElement instance = _assetInstantiator.Instantiate(prefab, parent, pos, Vector3.one);
            instance.Initialize();
            return instance;
        }

        public void Remove(View view)
        {
            if(view is null)
                return;

            if (view is PageView page)
            {
                _pages.Remove(page.Type);
            }

            if(view == null)
                return;

            view.Dispose();
            _assetInstantiator.Destroy(view.gameObject);
        }

        private string GetPathPage(PageType type)
        {
            return type switch
            {
                PageType.Game => AssetPath.GAME_PAGE,
                PageType.Loading => AssetPath.LOADING_PAGE,
                PageType.Error => AssetPath.ERROR_PAGE,
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