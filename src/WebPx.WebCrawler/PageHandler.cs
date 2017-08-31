using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{

    /// <summary>
    /// Registra una acción a ejecutar (Handler) para una Url específica
    /// </summary>
    public abstract class PageHandler : NavigationHandler, IPageHandle
    {

        public PageHandler(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// Gets the Url for the page to handle
        /// </summary>
        public string Url { get; private set; }

        public override bool CanHandle(NavigationResult navigation)
        {
            return string.Equals(this.Url, navigation?.Url);
        }

        public abstract void Handle(NavigationResult result);

    }
}
