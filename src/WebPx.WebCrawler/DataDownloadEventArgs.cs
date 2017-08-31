using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{
    public class DataDownloadEventArgs
    {
        internal DataDownloadEventArgs(/*BufferedDownloadHandler Handler, */byte[] data, int length)
        {
            //this.Handler = Handler;
            this.Data = data;
            this.Length = length;
        }

        //public BufferedDownloadHandler Handler { get; private set; }

        public byte[] Data { get; private set; }

        public int Length { get; private set; }
    }
}
