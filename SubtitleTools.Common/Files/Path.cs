using System.Windows.Forms;

namespace SubtitleTools.Common.Files
{
    public class Path
    {
        public static string AppPath
        {
            get
            {
                return  System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            }
        }
    }
}
