using System.Text.RegularExpressions;

namespace SubtitleTools.Common.Regex
{
    public static class RegexHelper
    {
        #region Fields (7)

        //regular expression to match numeric values
        private const string NumericPattern = "(^[-+]?\\d+(,?\\d*)*\\.?\\d*([Ee][-+]\\d*)?$)|(^[-+]?\\d?(,?\\d*)*\\.\\d+([Ee][-+]\\d*)?$)";
        private static readonly System.Text.RegularExpressions.Regex RegExNumeric = new System.Text.RegularExpressions.Regex(NumericPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private static readonly System.Text.RegularExpressions.Regex RegexStrip = new System.Text.RegularExpressions.Regex(@"<.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly System.Text.RegularExpressions.Regex RegexUploadUrl = new System.Text.RegularExpressions.Regex(@"(?s)<member>\s+<name>data</name>\s+<value>\s+<string>(.+?)</string>\s+</value>\s+</member>", RegexOptions.Compiled);
        public static readonly System.Text.RegularExpressions.Regex RgArrow = new System.Text.RegularExpressions.Regex(" --> ", RegexOptions.Compiled);
        public static readonly System.Text.RegularExpressions.Regex RgGroups = new System.Text.RegularExpressions.Regex("([0-9]+):([0-9]+):([0-9]+),([0-9]+)", RegexOptions.Compiled);
        //regular expression to match expressions like  "00:00:20,000 --> 00:00:24,400"
        private static readonly System.Text.RegularExpressions.Regex RgIsTime = new System.Text.RegularExpressions.Regex("[0-9]+:[0-9]+:[0-9]+,[0-9]+ --> [0-9]+", RegexOptions.Compiled);

        #endregion Fields

        #region Methods (5)

        // Public Methods (5)

        public static string GetUploadUrl(string xml)
        {
            var match = RegexUploadUrl.Match(xml);
            return match.Groups.Count == 2 ? match.Groups[1].Value : string.Empty;
        }

        public static bool IsNumeric(this string value)
        {
            return RegExNumeric.Match(value).Success;
        }

        public static bool IsTimeLine(this string value)
        {
            return RgIsTime.Match(value).Success;
        }

        #endregion Methods
    }
}
