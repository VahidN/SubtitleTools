using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SubtitleTools.Common.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            switch (value.ToString().ToLower())
            {
                case "announcement":
                    return new SolidColorBrush(Color.FromRgb(165, 239, 161));
                    break;
                case "alert":
                    return new SolidColorBrush(Color.FromRgb(239, 137, 5));
                case "info":
                    return new SolidColorBrush(Color.FromRgb(247, 241, 241));
                case "error":
                    return new SolidColorBrush(Color.FromRgb(237, 67, 67));
                default:
                    return new SolidColorBrush(Color.FromRgb(247, 241, 241));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
