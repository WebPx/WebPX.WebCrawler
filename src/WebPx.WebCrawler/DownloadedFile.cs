using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{
    public class DownloadedFile
    {
        internal DownloadedFile(string contentType, string url, byte[] data)
        {
            this.ContentType = contentType;
            this.Url = url;
            this.Data = data;
        }

        public string ContentType { get; private set; }

        public string Url { get; private set; }

        public byte[] Data { get; private set; }
    }
}
