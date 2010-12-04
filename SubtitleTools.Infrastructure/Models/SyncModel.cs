using System.ComponentModel;

namespace SubtitleTools.Infrastructure.Models
{
    public class SyncModel : INotifyPropertyChanged
    {
        #region Fields (5)

        int _addIndex;
        int _hour;
        int _milliseconds;
        int _minutes;
        int _seconds;

        #endregion Fields

        #region Properties (5)

        public int AddIndex
        {
            set
            {
                if (_addIndex == value) return;
                _addIndex = value;
                raisePropertyChanged("AddIndex");
            }
            get
            {
                return _addIndex;
            }
        }

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
