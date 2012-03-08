using System;
using System.IO;
using System.Speech.Recognition;
using SubtitleTools.Common.Logger;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.Core
{
    public class Sre
    {
        #region Fields (6)

        float _confidenceSum;
        readonly string _engineId;
        readonly string _filePath;
        TimeSpan _mediaLength = TimeSpan.Zero;
        int _number = 1;
        System.Speech.Recognition.SpeechRecognitionEngine _sre;

        #endregion Fields

        #region Constructors (1)

        public Sre(string filePath, string engineId)
        {
            _filePath = filePath;
            _engineId = engineId;
        }

        #endregion Constructors

        #region Properties (11)

        public Action<TimeSpan> AudioPositionChanged { set; get; }

        public Action<float> AverageConfidence { set; get; }

        public SilenceTimeout InitialSilenceTimeouts
        {
            get
            {
                if (_sre == null) return null;
                return new SilenceTimeout
                        {
                            BabbleTimeout = _sre.BabbleTimeout,
                            EndSilenceTimeout = _sre.EndSilenceTimeout,
                            EndSilenceTimeoutAmbiguous = _sre.EndSilenceTimeoutAmbiguous,
                            InitialSilenceTimeout = _sre.InitialSilenceTimeout
                        };
            }
        }

        public bool IsRunning { get; private set; }

        public TimeSpan MediaLength
        {
            get
            {
                if (_sre == null || string.IsNullOrWhiteSpace(_filePath)) return TimeSpan.Zero;
                return TimeSpan.FromSeconds(
                            new FileInfo(_filePath).Length /
                            (_sre.AudioFormat.SamplesPerSecond * _sre.AudioFormat.ChannelCount * _sre.AudioFormat.BitsPerSample / 8));
            }
        }

        public Action<int> Progress { set; get; }

        public bool RecognizeAsync { set; get; }

        public Action<string> RecognizeCompleted { set; get; }

        public Action<string> RecognizeEnd { set; get; }

        public Action<TimeSpan> RecognizerAudioPositionChanged { set; get; }

        public Action<SubtitleItem> SpeechRecognized { set; get; }

        #endregion Properties

        #region Methods (11)

        // Public Methods (3) 

        public void InitEngine()
        {
            _sre = new System.Speech.Recognition.SpeechRecognitionEngine(_engineId);
            _sre.SpeechRecognized += speechRecognized;
            _sre.RecognizeCompleted += recognizeCompleted;
            _sre.SpeechHypothesized += speechHypothesized;
            var grammar = new DictationGrammar();
            _sre.LoadGrammar(grammar);
            _sre.SetInputToWaveFile(_filePath);
        }

        public void StartRecognition(SilenceTimeout silenceTimeouts)
        {
            IsRunning = true;
            _confidenceSum = 0;
            _number = 1;
            setSilenceTimeouts(silenceTimeouts);
            if (RecognizeAsync)
                _sre.RecognizeAsync(RecognizeMode.Multiple);
            else
                _sre.Recognize();
        }

        public void StopRecognition()
        {
            IsRunning = false;
            if (_sre == null) return;
            _sre.RecognizeAsyncCancel();
        }
        // Private Methods (8) 

        void recognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            IsRunning = false;
            RecognizeEnd("Recognize Completed.");
            updatePosition();
            if (RecognizeCompleted == null) return;

            RecognizeCompleted("Recognize Completed.");

            if (e.Error != null)
                RecognizeCompleted(string.Format("Error encountered, {0}: {1}", e.Error.GetType().Name, e.Error.Message));

            if (e.Cancelled)
                RecognizeCompleted("Operation cancelled.");

            if (e.InputStreamEnded)
                RecognizeCompleted("End of stream encountered.");
        }

        private void setSilenceTimeouts(SilenceTimeout silenceTimeouts)
        {
            if (silenceTimeouts == null) return;
            try
            {
                _sre.BabbleTimeout = silenceTimeouts.BabbleTimeout;
                _sre.EndSilenceTimeout = silenceTimeouts.EndSilenceTimeout;
                _sre.EndSilenceTimeoutAmbiguous = silenceTimeouts.EndSilenceTimeoutAmbiguous;
                _sre.InitialSilenceTimeout = silenceTimeouts.InitialSilenceTimeout;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
        }

        void speechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            updatePosition();
        }

        void speechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result == null || string.IsNullOrWhiteSpace(e.Result.Text)) return;
            if (SpeechRecognized == null) return;
            SpeechRecognized(new SubtitleItem
                                {
                                    Dialog = ParseSrt.SetMaxWordsPerLine(e.Result.Text),
                                    Number = _number++,
                                    StartTs = e.Result.Audio.AudioPosition,
                                    EndTs = e.Result.Audio.AudioPosition + e.Result.Audio.Duration
                                });

            _confidenceSum += e.Result.Confidence;
            updateAverageConfidence();
            updatePosition();
        }

        private TimeSpan tryGetMediaLength()
        {
            if (_mediaLength.TotalSeconds == 0)
            {
                _mediaLength = MediaLength;
            }
            return _mediaLength;
        }

        private void updateAverageConfidence()
        {
            if (AverageConfidence == null) return;
            AverageConfidence(_confidenceSum / _number);
        }

        private void updatePosition()
        {
            if (_sre == null) return;
            try
            {
                if (AudioPositionChanged != null) AudioPositionChanged(_sre.AudioPosition);
                if (RecognizerAudioPositionChanged != null) RecognizerAudioPositionChanged(_sre.RecognizerAudioPosition);
                updateProgress();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
        }

        private void updateProgress()
        {
            var mediaLen = tryGetMediaLength();
            if (mediaLen.TotalSeconds == 0) return;
            if (Progress != null) Progress((int)((_sre.RecognizerAudioPosition.TotalSeconds / _mediaLength.TotalSeconds) * 100));
        }

        #endregion Methods
    }
}