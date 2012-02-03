using SubtitleTools.Common.MVVM;
using System;

namespace SubtitleTools.Infrastructure.Models
{
    public class SpeechRecognitionModel : ViewModelBase
    {
        #region Fields (10)

        AudioFormats _audioFormatsData = new AudioFormats();
        TimeSpan _audioLength;
        TimeSpan _audioPosition;
        float _confidence;
        string _fileName;
        int _progress;
        TimeSpan _recognizerAudioPosition;
        SpeechRecognitionEngine _selectedEngine;
        SilenceTimeout _silenceTimeoutData;
        SpeechRecognitionEngines _speechRecognitionEnginesData = new SpeechRecognitionEngines();

        #endregion Fields

        #region Properties (10)

        public AudioFormats AudioFormatsData
        {
            get { return _audioFormatsData; }
            set
            {
                _audioFormatsData = value;
                RaisePropertyChanged("AudioFormatsData");
            }
        }

        public TimeSpan AudioLength
        {
            get { return _audioLength; }
            set
            {
                _audioLength = value;
                RaisePropertyChanged("AudioLength");
            }
        }

        public TimeSpan AudioPosition
        {
            get { return _audioPosition; }
            set
            {
                _audioPosition = value;
                RaisePropertyChanged("AudioPosition");
            }
        }

        public float Confidence
        {
            get { return _confidence; }
            set
            {
                _confidence = value;
                RaisePropertyChanged("Confidence");
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                if (_progress == value) return;
                _progress = value;
                RaisePropertyChanged("Progress");
            }
        }

        public TimeSpan RecognizerAudioPosition
        {
            get { return _recognizerAudioPosition; }
            set
            {
                _recognizerAudioPosition = value;
                RaisePropertyChanged("RecognizerAudioPosition");
            }
        }

        public SpeechRecognitionEngine SelectedEngine
        {
            get { return _selectedEngine; }
            set
            {
                _selectedEngine = value;
                RaisePropertyChanged("SelectedEngine");
            }
        }

        public SilenceTimeout SilenceTimeoutData
        {
            get { return _silenceTimeoutData; }
            set
            {
                _silenceTimeoutData = value;
                RaisePropertyChanged("SilenceTimeoutData");
            }
        }

        public SpeechRecognitionEngines SpeechRecognitionEnginesData
        {
            get { return _speechRecognitionEnginesData; }
            set
            {
                _speechRecognitionEnginesData = value;
                RaisePropertyChanged("SpeechRecognitionEnginesData");
            }
        }

        #endregion Properties
    }
}