using System.IO;
using System.Xml.Serialization;

namespace SubtitleTools.Common.CodePlexRss
{
    public class Deserializer
    {
        public static rss MapToRss(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            var deserializer = new XmlSerializer(typeof(rss));
            return (rss)deserializer.Deserialize(new StringReader(xml));
        }
    }
}
