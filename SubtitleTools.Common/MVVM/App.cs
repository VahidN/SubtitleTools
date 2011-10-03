using System.Windows.Forms;

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
            get { return Application.StartupPath; }
        }
    }
}
