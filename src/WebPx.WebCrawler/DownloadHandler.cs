using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{
    public class DownloadHandler : NavigationHandler
    {
        public DownloadHandler()
        {

        }

        public override bool CanHandle(NavigationResult navigation)
        {
            return true;
        }

        ///// <summary>
        ///// Gets the handler procedure
        ///// </summary>
        //public Action<NavigationResult> Handler { get; private set; }

        public virtual void Handle(DownloadResult result)
        {
            long total = result.ContentLength;
            long received = 0;
            byte[] buffer = new byte[1024];
            byte[] data = new byte[total];
            using (Stream input = result.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(input))
                {
                    int size = 0;
                    do
                    {
                        size = input.Read(buffer, 0, buffer.Length);
                        Array.Copy(buffer, 0, data, received, size);
                        received += size;
                    } while (size > 0);
                }
            }
        }
    }
}
