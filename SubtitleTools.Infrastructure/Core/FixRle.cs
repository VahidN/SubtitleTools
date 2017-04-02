using System;
using System.Text;
using DNTPersianUtils.Core;

namespace SubtitleTools.Infrastructure.Core
{
    /// <summary>
    /// RLE or U+202B is the RIGHT-TO-LEFT EMBEDDING character
    /// http://www.w3.org/International/questions/qa-bidi-controls
    /// </summary>
    public static class FixRle
    {
        /// <summary>
        /// Makes the text inside right-to-left by surrounding it with RLE
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Processed Text</returns>
        public static string ApplyRle(this string text)
        {
            var newContent = new StringBuilder();
            const char RLEChar = (char)0x202B;

            var lines = text.Trim().Split('\n');
            foreach (var line in lines)
            {
                if (line.ContainsFarsi())
                {
                    var farsiLine = line.Trim();
                    farsiLine = removeRLE(RLEChar, farsiLine);
                    farsiLine = moveWrongPositionedSubtitleChars(farsiLine);
                    newContent.AppendFormat("{0}{1}{2}", RLEChar, farsiLine.Trim(), Environment.NewLine);
                }
                else
                    newContent.AppendFormat("{0}{1}", line.Trim(), Environment.NewLine);
            }

            return newContent.ToString().Trim();
        }

        private static string removeRLE(char RLEChar, string farsiLine)
        {
            if (farsiLine[0] == RLEChar)
                farsiLine = farsiLine.Remove(0, 1);
            return farsiLine;
        }

        private static string moveWrongPositionedSubtitleChars(string farsiLine)
        {
            var badStartingChars = new[] { "!", "...", "....", ".", "-", "?", "?!", "،", "؛", "؟", "؟!" };

            foreach (var badStartingChar in badStartingChars)
            {
                if (farsiLine.StartsWith(badStartingChar))
                    return string.Format("{0}{1}", farsiLine.Substring(badStartingChar.Length), badStartingChar);
            }

            return farsiLine;
        }
    }
}
