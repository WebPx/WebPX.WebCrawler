using WebPx.WebCrawler;

namespace WebPx.WebCrawler
{
    public interface IPageHandle
    {
        bool CanHandle(NavigationResult navigation);
        void Handle(NavigationResult result);
    }
}