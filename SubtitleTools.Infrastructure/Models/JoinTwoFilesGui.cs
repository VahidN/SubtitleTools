using System.ComponentModel;

namespace SubtitleTools.Infrastructure.Models
{
    public class JoinTwoFilesGui : INotifyPropertyChanged
    {
        #region Fields (5)

        int _hour;
        int _milliseconds;
        int _minutes;
        int _seconds;
        string _subtitlePath;

        #endregion Fields

        #region Properties (5)

        public int Hour
        {
            set
            {
                if (_hour == value) return;
                _hour = value;
                raisePropertyChanged("Hour");
            }
            get
            {
                return _hour;
            }
        }

        public int Milliseconds
        {
            set
            {
                if (_milliseconds == value) return;
                _milliseconds = value;
                raisePropertyChanged("Milliseconds");
            }
            get
            {
                return _milliseconds;
            }
        }

        public int Minutes
        {
            set
            {
                if (_minutes == value) return;
                _minutes = value;
                raisePropertyChanged("Minutes");
            }
            get
            {
                return _minutes;
            }
        }

        public int Seconds
        {
            set
            {
                if (_seconds == value) return;
                _seconds = value;
                raisePropertyChanged("Seconds");
            }
            get
            {
                return _seconds;
            }
        }

        public string SubtitlePath
        {
            get { return _subtitlePath; }
            set
            {
                if (_subtitlePath == value) return;
                _subtitlePath = value;
                raisePropertyChanged("SubtitlePath");
            }
        }

        #endregion Properties



        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        void raisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
