using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebPx.WebCrawler
{
    public class SiteCrawler : Component
    {
        public SiteCrawler()
        {
                
        }

        private CredentialCache _credentials;

        private CredentialCache GetCredentials()
        {
            if (_credentials == null)
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                CredentialCache credentialCache = new CredentialCache();
                _credentials = credentialCache;
            }
            return _credentials;
        }

        public CredentialCache Credentials
        {
            get
            {
                return GetCredentials();
            }
        }

        private CookieContainer _cookies = new CookieContainer();

        public CookieContainer Cookies
        {
            get
            {
                return _cookies;
            }
        }

        public void NavigateTo(string url, string contentType = null, string content = null, string method = null)
        {
            internalNavigateTo(url, contentType, content, method, HandleResponse);
        }

        public void NavigateTo(string url, NameValueCollection formValues,  string method = null)
        {
            string contentType = null;
            string content = null;

            var sb = new StringBuilder();
            if (formValues!=null && formValues.Count > 0)
            {
                bool first = true;
                foreach (var key in formValues.AllKeys)
                {
                    if (first)
                        first = false;
                    else
                        sb.Append("&");
                    sb.Append(key);
                    var value = formValues[key];
                    if (!string.IsNullOrEmpty(WebUtility.UrlEncode(value)))
                    {
                        sb.Append("=");
                        sb.Append(WebUtility.UrlEncode(value));
                    }
                }
            }

            switch (method)
            {
                case WebRequestMethods.Http.Get:
                    if (sb.Length > 0)
                        url = string.Format("{0}{1}{2}", url, url.Contains("?") ? "&" : "?", sb.ToString());
                    break;
                case WebRequestMethods.Http.Post:
                    contentType = "application/x-www-form-urlencoded";
                    content = sb.ToString();
                    break;
            }

            NavigateTo(url, contentType, content, method);
        }

        public void Download(string url, string contentType = null, string content = null, string method = null, DownloadHandler downloadHandler = null)
        {
            internalNavigateTo(url, contentType, content, method, (webResponse) => HandleDownload(webResponse, downloadHandler ?? DownloadHandler));
        }

        private DownloadHandler _downloadHandler = new DownloadHandler();

        public DownloadHandler DownloadHandler
        {
            get { return _downloadHandler; }
            set { _downloadHandler = value; }
        }

        public void HandleDownload(WebResponse webResponse, DownloadHandler downloadHandler)
        {
            var download = new DownloadResult(webResponse);
            download.Headers = webResponse.Headers;
            download.ContentType = webResponse.ContentType;
            download.ContentLength = webResponse.ContentLength;
            downloadHandler.Handle(download);
        }

        private void internalNavigateTo(string url, string contentType, string content, string method, Action<WebResponse> responseHandler)
        { 
            var uri = new Uri(url);

            WebResponse response = null;
            var request = (HttpWebRequest)WebRequest.CreateDefault(uri);
            if (request is HttpWebRequest httpWebRequest)
            {
                request.CookieContainer = Cookies;
                request.AllowAutoRedirect = false;
            }
            request.Credentials = GetCredentials();
            request.Method = method ?? WebRequestMethods.Http.Get;
            if (content != null)
            {

                byte[] buffer = null;
                request.ContentType = contentType;
                request.ContentLength = buffer?.LongLength ?? 0;
                if (buffer != null)
                {
                    var reqStream = request.GetRequestStream();
                    reqStream.Write(buffer, 0, buffer.Length);
                    reqStream.Close();
                }
            }
            try
            {
                response = request.GetResponse();
                
                if (response != null)
                {
                    var http = (HttpWebResponse)response;
                    responseHandler.Invoke(response);
                }
            }
            catch (WebException we)
            {
                var http = (HttpWebResponse)we.Response;
            }
        }

        private void HandleResponse(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    string html = reader.ReadToEnd();

                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var result = new NavigationResult(doc, null, response.ResponseUri.ToString())
                    {
                        ContentType = response.ContentType,
                        Headers = response.Headers,
                        ContentLength = response.ContentLength
                    };
                    HandleRequest(result);
                }
            }
        }

        //
        // Summary:
        //     Gets an event log you can use to write notification of service command calls,
        //     such as Start and Stop, to the Application event log.
        //
        // Returns:
        //     An System.Diagnostics.EventLog instance whose source is registered to the Application
        //     log.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual EventLog EventLog { get; set; }

        private bool HandleRequest(NavigationResult result)
        {
            foreach (var pageHandler in pageHandlers)
                if (pageHandler.CanHandle(result))
                {
                    try
                    {
                        pageHandler.Handle(result);
                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        // Si capturamos una excepción la registramos y salimos cerrando la ventana
                        this.EventLog?.WriteEntry(string.Format("Exception: {0}", ex.Message), EventLogEntryType.Error, 1);
                        break;
                    }
                }
            return false;
        }

        private List<IPageHandle> pageHandlers = new List<IPageHandle>();

        public void AddHandler(IPageHandle pageHandler)
        {
            pageHandlers.Add(pageHandler);
        }
    }
}
