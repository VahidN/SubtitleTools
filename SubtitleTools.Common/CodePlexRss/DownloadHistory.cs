﻿using System;
using System.Text;
using SubtitleTools.Common.CodePlexRss.Model;
using SubtitleTools.Common.Files;
using SubtitleTools.Common.Regex;

namespace SubtitleTools.Common.CodePlexRss
{
    public class DownloadHistory
    {
        #region Methods (2)

        // Public Methods (1) 

        public static VersionsInfo ParseInfo(string rssXml)
        {
            var result = new VersionsInfo();
            var rss = Deserializer.MapToRss(rssXml);

            var channels = rss.channel;
            foreach (var channel in channels)
            {
                var items = channel.item;
                foreach (var item in items)
                {
                    var author = item.author.StripHtmlTags().UnescapeXml();
                    if (string.IsNullOrWhiteSpace(author))
                    {
                        continue;
                    }

                    var desc = fixDescription(item);
                    result.Add(
                        new VersionInfo
                        {
                            Author = author,
                            Description = desc.UnescapeXml(),
                            Link = item.link.StripHtmlTags(),
                            PubDate = DateTime.Parse(item.pubDate),
                            Title = item.title.StripHtmlTags().UnescapeXml()
                        });
                }
            }

            return result;
        }
        // Private Methods (1) 

        private static string fixDescription(rssChannelItem item)
        {
            var desc = item.description.StripHtmlTags();
            var lines = desc.Split('\n');
            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                sb.AppendLine(line.Trim());
            }
            return sb.ToString();
        }

        #endregion Methods
    }
}
