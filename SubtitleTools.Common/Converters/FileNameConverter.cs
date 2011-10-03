using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace SubtitleTools.Common.Converters
{
    public class FileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var path = value.ToString();
            if (value is Uri)
                path = ((Uri)value).LocalPath;
            return !File.Exists(path) ? value : Path.GetFileName(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
