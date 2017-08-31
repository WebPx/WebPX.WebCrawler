using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{
    public abstract class NavigationHandler
    {
        public NavigationHandler()
        {

        }

        public abstract bool CanHandle(NavigationResult navigation);

    }
}
