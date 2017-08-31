using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{
    public sealed class NavigationResult
    {
        internal NavigationResult(HtmlDocument document, object state, string Url)
        {
            this.Document = document;
            this.State = state;
            this.Url = Url;
        }

        public object State { get; private set; }

        public string ContentType { get; internal set; }

        public WebHeaderCollection Headers { get; internal set; }

        public HtmlDocument Document { get; private set; }

        public string Url { get; private set; }

        public long ContentLength { get; internal set; }

        internal SiteCrawler PageCrawler { get; set; }

        public void NavigateTo(string url, NameValueCollection formValues = null, string method = null)
        {
            PageCrawler?.NavigateTo(url, formValues, method);
        }

        public void NavigateTo(string url, string contentType = null, string content = null, string method = null)
        {
            PageCrawler?.NavigateTo(url, contentType, content, method);
        }

    }

}
