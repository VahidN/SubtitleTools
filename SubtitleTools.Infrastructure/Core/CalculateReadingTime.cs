using System;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.Core
{
    public static class CalculateReadingTime
    {
        public static TimeSpan MinReadTime(this string text, int wordsPerSecond = 3)
        {
            var wordsCount = text.WordsCount();
            var seconds = wordsCount / wordsPerSecond;
            return TimeSpan.FromSeconds(seconds);
        }

        public static void FixMinReadTime(this SubtitleItem subtitleItem)
        {
            var totalSeconds = (subtitleItem.EndTs - subtitleItem.StartTs).TotalSeconds;
            var readTime = subtitleItem.Dialog.MinReadTime();
            var minReadTime = readTime.TotalSeconds;

            if(totalSeconds< minReadTime)
            {
                subtitleItem.EndTs = subtitleItem.StartTs.Add(readTime);
            }
        }
    }
}