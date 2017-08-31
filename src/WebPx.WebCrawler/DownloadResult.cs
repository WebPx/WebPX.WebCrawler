using System;
using System.IO;
using System.Net;

namespace WebPx.WebCrawler
{
    public class DownloadResult
    {
        public DownloadResult(WebResponse webResponse)
        {
            this._webResponse = webResponse;
        }

        private WebResponse _webResponse;

        public WebHeaderCollection Headers { get; internal set; }

        public string ContentType { get; internal set; }

        public long ContentLength { get; internal set; }

        public Stream GetResponseStream()
        {
            return _webResponse.GetResponseStream();
        }
    }
}