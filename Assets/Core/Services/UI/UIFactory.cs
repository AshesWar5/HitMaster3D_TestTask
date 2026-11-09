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
            if (page != null)
            {
                if(!page.IsInitialized)
                    page.Initialize();

                return page as TypePage;
            }

            _pages.Remove(type);

            var prefab = GetPage<TypePage>(type);
            var instance = _assetInstantiator.Instantiate(prefab, parent, pos, Vector3.one);
            instance.Initialize();
            _pages.Add(type, instance);
            return instance;
        }

        TTypeElement IUIElementFactory.Create<TTypeElement>(UIElementType type, Vector3 pos = new Vector3(),
            Transform parent = null)
        {
            var prefab = GetElement<TTypeElement>(type);
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

        private TPage GetPage<TPage>(PageType type) where TPage : PageView
        {
            return type switch
            {
                PageType.Game => _assetProvider.Get<TPage>(AssetAddress.GAME_PAGE),
                PageType.Loading => _assetProvider.Load<TPage>(AssetPath.LOADING_PAGE),
                PageType.Error => _assetProvider.Load<TPage>(AssetPath.ERROR_PAGE),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        private TElement GetElement<TElement>(UIElementType type) where TElement : ViewUI
        {
            return type switch
            {
                UIElementType.HealthEnemy => _assetProvider.Get<TElement>(AssetAddress.HEALTH_ENEMY_UI_ELEMENT),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}