
namespace SubtitleTools.Common.MVVM
{
    public class App
    {
        readonly static Messenger _messenger = new Messenger();

        public static Messenger Messenger
        {
            get { return _messenger; }
        }   
    }
}
