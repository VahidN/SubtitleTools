using System;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Infrastructure.Models
{
    public class SilenceTimeout : ViewModelBase
    {
        #region Fields (4)

        TimeSpan _babbleTimeout = TimeSpan.Zero;
        TimeSpan _endSilenceTimeout = TimeSpan.Zero;
        TimeSpan _endSilenceTimeoutAmbiguous = TimeSpan.Zero;
        TimeSpan _initialSilenceTimeout = TimeSpan.Zero;

        #endregion Fields

        #region Properties (4)

        /// <summary>
        /// the time interval during which a SpeechRecognitionEngine accepts input containing only background noise, before finalizing recognition.
        /// </summary>
        public TimeSpan BabbleTimeout
        {
            get { return _babbleTimeout; }
            set
            {
                _babbleTimeout = value;
                RaisePropertyChanged("BabbleTimeout");
            }
        }

        /// <summary>
        /// the interval of silence that the SpeechRecognitionEngine will accept at the end of unambiguous input before finalizing a recognition operation.
        /// </summary>
        public TimeSpan EndSilenceTimeout
        {
            get { return _endSilenceTimeout; }
            set
            {
                if (value.TotalSeconds >= 100)
                    throw new InvalidOperationException("Property cannot be negative or greater than 100 seconds.");

                _endSilenceTimeout = value;
                RaisePropertyChanged("EndSilenceTimeout");
            }
        }

        /// <summary>
        /// the interval of silence that the SpeechRecognitionEngine will accept at the end of ambiguous input before finalizing a recognition operation.
        /// </summary>
        public TimeSpan EndSilenceTimeoutAmbiguous
        {
            get { return _endSilenceTimeoutAmbiguous; }
            set
            {
                if (value.TotalSeconds >= 100)
                    throw new InvalidOperationException("Property cannot be negative or greater than 100 seconds.");

                _endSilenceTimeoutAmbiguous = value;
                RaisePropertyChanged("EndSilenceTimeoutAmbiguous");
            }
        }

        /// <summary>
        /// the time interval during which a SpeechRecognitionEngine accepts input containing only silence before finalizing recognition.
        /// </summary>
        public TimeSpan InitialSilenceTimeout
        {
            get { return _initialSilenceTimeout; }
            set
            {
                _initialSilenceTimeout = value;
                RaisePropertyChanged("InitialSilenceTimeout");
            }
        }

        #endregion Properties
    }
}