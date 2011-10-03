using System;
using SubtitleTools.Common.Net;

namespace SubtitleTools.Common.Toolkit
{
    public class Imdb
    {
        public static long GetImdbId(string movieName)
        {
            string url = "http://www.google.com/search?q=site:imdb.com%20" + Uri.EscapeUriString(movieName);
            string html = Downloader.FetchWebPage(url);

            var match = new System.Text.RegularExpressions.Regex(@"<a href=""(http://www.imdb.com/title/tt\d{7}/)"".*?>.*?</a>").Match(html);
            if (match.Success)
            {
                var link = match.Groups[1].Value;
                match = new System.Text.RegularExpressions.Regex(@"http://www.imdb.com/title/(tt\d{7})/").Match(link);
                if (match.Success)
                {
                    return long.Parse(match.Groups[1].Value.Replace("tt", string.Empty));
                }
            }

            return 0;
        }
    }
}
