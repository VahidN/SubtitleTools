using System;
using System.ComponentModel;
using System.Windows;
using SubtitleTools.Infrastructure.Core;

namespace SubtitleTools.Infrastructure.Models
{
    public class SubtitleItem : INotifyPropertyChanged
    {
        #region Fields (7)

        int _caretIndex;
        string _dialog;
        FlowDirection _dialogFlowDirection = FlowDirection.LeftToRight;
        TimeSpan _endTs = TimeSpan.Zero;
        int _number;
        TimeSpan _startTs = TimeSpan.Zero;
        string _time;

        #endregion Fields

        #region Properties (7)

        public int CaretIndex
        {
            get { return _caretIndex; }
            set
            {
                if (_caretIndex == value) return;
                _caretIndex = value;
                raisePropertyChanged("CaretIndex");
            }
        }

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
                if (string.IsNullOrWhiteSpace(_dialog)) return string.Empty;
                return _dialog;
            }
        }

        public FlowDirection DialogFlowDirection
        {
            get { return _dialogFlowDirection; }
            set
            {
                if (_dialogFlowDirection == value) return;
                _dialogFlowDirection = value;
                raisePropertyChanged("DialogFlowDirection");
            }
        }

        public TimeSpan EndTs
        {
            get { return _endTs == TimeSpan.Zero ? new TimeSpan(0, 0, 0, 0, 310) : _endTs; }
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
            get { return _startTs == TimeSpan.Zero ? new TimeSpan(0, 0, 0, 0, 310) : _startTs; }
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
                return string.IsNullOrWhiteSpace(this._time) ? string.Empty : this._time;
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
