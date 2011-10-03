using System;
using System.ComponentModel;
using System.Windows;

namespace SubtitleTools.Infrastructure.Models
{
    public class PreviewModel : INotifyPropertyChanged
    {
        #region Fields (16)

        string _dialog;
        FlowDirection _dialogFlowDirection = FlowDirection.LeftToRight;
        bool _dragCompleted;
        string _mediaError;
        string _mediaLen;
        TimeSpan _mediaManualPosition = TimeSpan.Zero;
        TimeSpan _mediaNaturalDuration = TimeSpan.Zero;
        TimeSpan _mediaPosition = TimeSpan.Zero;
        bool _pauseMedia;
        string _playImage;
        bool _playMedia;
        double _seekBarLargeChange;
        double _seekBarMaximum;
        double _seekBarValue;
        bool _stopMedia;
        double _volumeSliderValue = 0.5;

        #endregion Fields

        #region Properties (16)

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
                return string.IsNullOrWhiteSpace(this._dialog) ? string.Empty : this._dialog;
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

        public bool DragCompleted
        {
            get { return _dragCompleted; }
            set
            {
                _dragCompleted = value;
                raisePropertyChanged("DragCompleted");
            }
        }

        public string MediaError
        {
            set
            {
                if (_mediaError == value) return;
                _mediaError = value;
                raisePropertyChanged("MediaError");
            }
            get { return _mediaError; }
        }

        public string MediaLen
        {
            set
            {
                if (_mediaLen == value) return;
                _mediaLen = value;
                raisePropertyChanged("MediaLen");
            }
            get { return _mediaLen; }
        }

        public TimeSpan MediaManualPosition
        {
            set
            {
                if (_mediaManualPosition.TotalSeconds == value.TotalSeconds) return;
                _mediaManualPosition = value;
                raisePropertyChanged("MediaManualPosition");
            }
            get { return _mediaManualPosition; }
        }

        public TimeSpan MediaNaturalDuration
        {
            set
            {
                if (_mediaNaturalDuration.TotalSeconds == value.TotalSeconds) return;
                _mediaNaturalDuration = value;
                raisePropertyChanged("MediaNaturalDuration");
            }
            get { return _mediaNaturalDuration; }
        }

        public TimeSpan MediaPosition
        {
            set
            {
                if (_mediaPosition.TotalSeconds == value.TotalSeconds) return;
                _mediaPosition = value;
                raisePropertyChanged("MediaPosition");
            }
            get { return _mediaPosition; }
        }

        public bool PauseMedia
        {
            set
            {
                _pauseMedia = value;
                raisePropertyChanged("PauseMedia");
            }
            get { return _pauseMedia; }
        }

        public string PlayImage
        {
            get { return _playImage; }
            set
            {
                if (_playImage == value) return;
                _playImage = value;
                raisePropertyChanged("PlayImage");
            }
        }

        public bool PlayMedia
        {
            set
            {
                _playMedia = value;
                raisePropertyChanged("PlayMedia");
            }
            get { return _playMedia; }
        }

        public double SeekBarLargeChange
        {
            get { return _seekBarLargeChange; }
            set
            {
                if (_seekBarLargeChange == value) return;
                _seekBarLargeChange = value;
                raisePropertyChanged("SeekBarLargeChange");
            }
        }

        public double SeekBarMaximum
        {
            get { return _seekBarMaximum; }
            set
            {
                if (_seekBarMaximum == value) return;
                _seekBarMaximum = value;
                raisePropertyChanged("SeekBarMaximum");
            }
        }

        public double SeekBarValue
        {
            set
            {
                if (_seekBarValue == value) return;
                _seekBarValue = value;
                raisePropertyChanged("SeekBarValue");
            }
            get { return _seekBarValue; }
        }

        public bool StopMedia
        {
            set
            {
                _stopMedia = value;
                raisePropertyChanged("StopMedia");
            }
            get { return _stopMedia; }
        }

        public double VolumeSliderValue
        {
            set
            {
                if (_volumeSliderValue == value) return;
                _volumeSliderValue = value;
                raisePropertyChanged("VolumeSliderValue");
            }
            get { return _volumeSliderValue; }
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
