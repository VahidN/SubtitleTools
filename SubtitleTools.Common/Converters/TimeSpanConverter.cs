using System;
using System.Globalization;
using System.Windows.Data;
using SubtitleTools.Common.Regex;

namespace SubtitleTools.Common.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var timeSpan = (TimeSpan)value;
            return string.Format("{0}:{1}:{2},{3}",
                timeSpan.Hours.ToString("D2"),
                timeSpan.Minutes.ToString("D2"),
                timeSpan.Seconds.ToString("D2"),
                timeSpan.Milliseconds.ToString("D3"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var m = RegexHelper.RgGroups.Match(value.ToString());
            var hh = int.Parse(m.Groups[1].ToString());
            var mm = int.Parse(m.Groups[2].ToString());
            var ss = int.Parse(m.Groups[3].ToString());
            var ff = int.Parse(m.Groups[4].ToString());

            return new TimeSpan(0, hh, mm, ss, ff);
        }
    }
}
