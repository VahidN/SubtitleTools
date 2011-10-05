using System.IO;

namespace SubtitleTools.Common.MVVM
{
    public class App
    {
        readonly static Messenger _messenger = new Messenger();

        public static Messenger Messenger
        {
            get { return _messenger; }
        }

        public static string Path
        {
            get { return System.Windows.Forms.Application.StartupPath; }
        }

        public static string StartupFileName
        {
            get
            {
                var startupFileName = System.Windows.Application.Current.Properties["StartupFileName"];
                if (startupFileName == null) return string.Empty;
                if (string.IsNullOrEmpty(startupFileName.ToString())) return string.Empty;
                if (!File.Exists(startupFileName.ToString())) return string.Empty;
                return startupFileName.ToString();
            }
        }
    }
}
