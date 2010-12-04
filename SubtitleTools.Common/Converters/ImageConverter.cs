using System;
using System.Globalization;
using System.Windows.Data;
using SubtitleTools.Common.ResourceFiles;

namespace SubtitleTools.Common.Converters
{
    //Must create DependencySource on same Thread as the DependencyObject
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            switch (value.ToString().ToLower())
            {
                case "announcement":
                    return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Images/event16.png");
                case "alert":
                    return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Images/alert.png");
                case "info":
                    return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Images/info.png");
                case "error":
                    return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Images/close.png");
                default:
                    return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Images/info.png");
            }           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
