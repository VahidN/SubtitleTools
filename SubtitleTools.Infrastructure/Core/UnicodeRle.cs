using System;
using SubtitleTools.Common.Regex;

namespace SubtitleTools.Infrastructure.Core
{
    public class UnicodeRle
    {
        public static string InsertBefore(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            if (!text.ContainsFarsi()) return text;

            const char rleChar = (char)0x202B;

            var lines = text.Trim().Split('\n');
            var newContent = string.Empty;
            foreach (var line in lines)
            {
                newContent += rleChar + line.Trim() + Environment.NewLine;
            }

            return newContent.Trim();
        }
    }
}
