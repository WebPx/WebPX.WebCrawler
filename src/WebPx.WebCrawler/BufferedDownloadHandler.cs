using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{
    public class BufferedDownloadHandler : DownloadHandler
    {
        public BufferedDownloadHandler()
        {
            this.BufferSize = 1024;
        }

        [DefaultValue(1024)]
        public int BufferSize { get; set; }

        public override void Handle(DownloadResult result)
        {
            long total = 0;
            long received = 0;
            byte[] buffer = new byte[1024];
            DownloadStarted.Invoke(this, EventArgs.Empty);
            using (Stream input = result.GetResponseStream())
            {
                total = input.Length;

                int size = input.Read(buffer, 0, buffer.Length);
                while (size > 0)
                {
                    ProcessBlock(buffer, size);
                    received += size;
                    size = input.Read(buffer, 0, buffer.Length);
                }
            }
            DownloadFinished.Invoke(this, EventArgs.Empty);
        }

        private void ProcessBlock(byte[] buffer, int size)
        {
            var args = new DataDownloadEventArgs(buffer, size);
            BlockDownloaded?.Invoke(this, args);
        }

        public event DataDownloadEventHandler BlockDownloaded;
        public event EventHandler DownloadStarted;
        public event EventHandler DownloadFinished;
    }
}
