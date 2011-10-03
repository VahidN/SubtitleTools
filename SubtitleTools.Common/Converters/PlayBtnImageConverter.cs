using System;
using System.Globalization;
using System.Windows.Data;
using SubtitleTools.Common.ResourceFiles;

namespace SubtitleTools.Common.Converters
{
    public class PlayBtnImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            switch (value.ToString().ToLower())
            {
                case "play":
                    return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Images/debug-run-icon.png");
                case "pause":
                    return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Images/debug-pause-icon.png");
                default:
                    return ResourceUtils.GetImgFromResource("SubtitleTools.Shell", "Images/debug-run-icon.png");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
