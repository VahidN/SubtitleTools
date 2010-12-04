using System;
using System.Windows.Media.Imaging;
using SubtitleTools.Common.Logger;

namespace SubtitleTools.Common.ResourceFiles
{
    public class ResourceUtils
    {
        public static BitmapImage GetImgFromResource(string app, string name)
        {
            try
            {
                var logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(string.Format("pack://application:,,,/{1};component/{0}", name, app));
                logo.EndInit();
                return logo;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                return null;
            }
        }       
    }
}
