using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{
    public class GenericPageHandler : NavigationHandler
    {
        public GenericPageHandler(Action<NavigationResult> handler)
        {
            this.Handler = handler;
        }

        public override bool CanHandle(NavigationResult navigation)
        {
            return true;
        }

        /// <summary>
        /// Gets the handler procedure
        /// </summary>
        public Action<NavigationResult> Handler { get; private set; }

        public virtual void Handle(NavigationResult result)
        {
            Handler?.Invoke(result);
        }

    }
}
