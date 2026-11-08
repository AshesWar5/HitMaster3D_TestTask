using System.Collections.Generic;
using Core.Services.UI;

namespace Core.Services.Page
{
    public sealed class PageService : IPageService
    {
        private readonly IUIPageFactory _factory;
        private readonly Stack<PageType> _pages;

        public PageService(IUIPageFactory factory)
        {
            _factory = factory;
            _pages = new Stack<PageType>();
        }

        public void Initialize() { }

        public void Dispose() { }

        public TPage Create<TPage>(PageType type) where TPage : PageView
        {
            PageView page = InstantiatePage(type);
            if (page is not null)
            {
                if (!page.IsFree)
                {
                    page.Hide();
                }
            }

            return page as TPage;
        }

        public void Open(PageType type) => Open(type, true);

        public void Close(bool isPop = true)
        {
            if(_pages.Count <= 0)
                return;

            PageType type = _pages.Peek();

            Close(type, false, isPop);
        }
        
        public void Close(PageType type, bool isFree = true,
            bool isOpenPage = false)
        {
            PageView page = InstantiatePage(type);
            if (page is not null && page.IsFree == isFree)
            {
                page.Hide();
                page.Exit();
            }
            
            if (_pages.Count > 0 && isOpenPage)
            {
                _pages.Pop();
            }

            if(_pages.Count > 0 && isOpenPage)
                Open(_pages.Peek(), false);
        }

        public TPage Get<TPage>(PageType type) where TPage : PageView
        {
            _factory.Pages.TryGetValue(type, out PageView page);
            return page as TPage;
        }

        public void Remove(PageView page) => _factory.Remove(page);

        private void Open(PageType type, bool isClosePage)
        {
            PageView page = InstantiatePage(type);
            if (page is not null)
            {
                if (!page.IsFree)
                {
                    if (isClosePage)
                    {
                        Close(false);
                    }

                    if (_pages.Count > 0)
                    {
                        if (_pages.Peek() != type)
                        {
                            _pages.Push(type);
                        }
                    }
                    else
                    {
                        _pages.Push(type);
                    }
                }
                
                page.Show();
                page.Enter();
            }
        }

        private PageView InstantiatePage(PageType type) =>
            _factory.Create<PageView>(type, parent: null);
    }
}