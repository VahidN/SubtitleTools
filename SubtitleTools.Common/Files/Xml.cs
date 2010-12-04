
namespace SubtitleTools.Common.Files
{
    public static class Xml
    {
        public static string EscapeXml(this string s)
        {
            var xml = s;
            if (!string.IsNullOrEmpty(xml))
            {
                // replace literal values with entities
                xml = xml.Replace("&", "&amp;");
                xml = xml.Replace("<", "&lt;");
                xml = xml.Replace(">", "&gt;");
                xml = xml.Replace("\"", "&quot;");
                xml = xml.Replace("'", "&apos;");
            }
            return xml;
        }


        public static string UnescapeXml(this string s)
        {
            var unxml = s;
            if (!string.IsNullOrEmpty(unxml))
            {
                // replace entities with literal values
                unxml = unxml.Replace("&apos;", "'");
                unxml = unxml.Replace("&quot;", "\"");
                unxml = unxml.Replace("&gt;", ">");
                unxml = unxml.Replace("&lt;", "<");
                unxml = unxml.Replace("&amp;", "&");
            }
            return unxml;
        }
    }
}
