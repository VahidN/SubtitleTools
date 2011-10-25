using System.ComponentModel;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Infrastructure.Models
{
    public class SyncModel : ViewModelBase
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
                RaisePropertyChanged("AddIndex");
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
                RaisePropertyChanged("Hour");
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
                RaisePropertyChanged("Milliseconds");
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
                RaisePropertyChanged("Minutes");
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
                RaisePropertyChanged("Seconds");
            }
            get
            {
                return _seconds;
            }
        }

        #endregion Properties
    }
}
