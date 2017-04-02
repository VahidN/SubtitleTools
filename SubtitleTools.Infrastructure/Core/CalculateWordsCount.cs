using System;
using System.Text.RegularExpressions;

namespace SubtitleTools.Infrastructure.Core
{
    public static class CalculateWordsCount
    {
        private static readonly Regex _matchAllTags =
            new Regex(@"<(.|\n)*?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static int WordsCount(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            text = text.cleanTags().Trim();
            text = text.Replace("\t", " ");
            text = text.Replace("\n", " ");
            text = text.Replace("\r", " ");

            var words = text.Split(
                new[] { ' ', ',', ';', '.', '!', '"', '(', ')', '?', ':', '\'', '«', '»', '+', '-' },
                StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        private static string cleanTags(this string data)
        {
            return data.Replace("\n", "\n ").removeHtmlTags();
        }

        private static string removeHtmlTags(this string text)
        {
            return string.IsNullOrEmpty(text) ?
                        string.Empty :
                        _matchAllTags.Replace(text, " ").Replace("&nbsp;", " ");
        }
    }
}