using System;
using System.Globalization;
using System.Windows.Data;

namespace SubtitleTools.Common.Converters
{
    public class NumberToTimeSpanConverter :  IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var number = (double)value;
            return TimeSpan.FromSeconds(number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
