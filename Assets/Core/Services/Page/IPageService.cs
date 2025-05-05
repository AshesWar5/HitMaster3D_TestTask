using System;
using Core.Infrastructures;

namespace Core.Services.Page
{
    public interface IPageService : IInitialize, IDisposable
    {
        TPage Create<TPage>(PageType type) where TPage : PageView;
        
        void Open(PageType type);

        void Close(bool isPop = true);

        void Close(PageType type, bool isFree = true,
            bool isOpenPage = false);

        TPage Get<TPage>(PageType type) where TPage : PageView;

        void Remove(PageView page);
    }
}