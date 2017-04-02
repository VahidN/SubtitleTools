using System;
using System.Globalization;
using System.Windows.Data;
using SubtitleTools.Common.ResourceFiles;

namespace SubtitleTools.Common.Converters
{
    public class Iso639Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Flags/null.png");
            }
            
            var image = ResourceUtils.GetImgFromResource("SubtitleTools.Shell", string.Format("Flags/{0}.png", value.ToString().ToLower()));
            if (image == null)
            {
                image = ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Flags/null.png");                
            }
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
