using System.IO;
using System.Net;
using System.Text;

namespace SubtitleTools.Common.Net
{
    public class PostXml
    {
        public static string PostData(string uri, string data, string contentType = "text/xml")
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.ContentType = contentType;
            webRequest.Method = "POST";
            webRequest.ServicePoint.Expect100Continue = false;
            
            var dataBytes = Encoding.UTF8.GetBytes(data);
            webRequest.ContentLength = dataBytes.Length;

            //Send it
            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(dataBytes, 0, dataBytes.Length);          
            }

            // get the response
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            using (var sr = new StreamReader(webResponse.GetResponseStream()))
            {
                return sr.ReadToEnd().Trim();
            }
        }
    }
}
