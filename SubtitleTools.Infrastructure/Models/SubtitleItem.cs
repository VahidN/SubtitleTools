using System.ComponentModel;
using System;
using SubtitleTools.Infrastructure.Core;

namespace SubtitleTools.Infrastructure.Models
{
    public class SubtitleItem : INotifyPropertyChanged
    {
        #region Fields (5)

        string _dialog;
        TimeSpan _endTs;
        int _number;
        TimeSpan _startTs;
        string _time;

        #endregion Fields

        #region Properties (5)

        public string Dialog
        {
            set
            {
                if (_dialog == value) return;
                _dialog = value;
                raisePropertyChanged("Dialog");
            }
            get
            {
                return _dialog;
            }
        }

        public TimeSpan EndTs
        {
            get { return _endTs; }
            set
            {
                _endTs = value;
                Time = string.Format("{0} --> {1}", ParseSrt.TimeSpanToString(StartTs), ParseSrt.TimeSpanToString(EndTs));
                raisePropertyChanged("EndTs");
            }
        }

        public int Number
        {
            set
            {
                if (_number == value) return;
                _number = value;
                raisePropertyChanged("Number");
            }
            get
            {
                return _number;
            }
        }

        public TimeSpan StartTs
        {
            get { return _startTs; }
            set
            {
                _startTs = value;
                Time = string.Format("{0} --> {1}", ParseSrt.TimeSpanToString(StartTs), ParseSrt.TimeSpanToString(EndTs));
                raisePropertyChanged("StartTs");
            }
        }

        public string Time
        {
            set
            {
                if (_time == value) return;
                _time = value;
                raisePropertyChanged("Time");
            }
            get
            {
                return _time;
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
