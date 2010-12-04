using System;
using System.IO;
using System.Net;

namespace SubtitleTools.Common.Net
{
    public class Downloader
    {
        public static string FetchWebPage(string url)
        {
            var uri = new Uri(url);
            var request = WebRequest.Create(uri) as HttpWebRequest;
            if (request == null) return string.Empty;
            request.Method = WebRequestMethods.Http.Get;
            request.AllowAutoRedirect = true;
            request.Timeout = 1000 * 300;
            request.KeepAlive = false;
            request.ReadWriteTimeout = 1000 * 300;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }
    }
}
