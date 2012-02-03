using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Speech.Recognition;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class SpeechRecognitionViewModel
    {
        #region Fields (1)

        private Sre _sre;

        #endregion Fields

        #region Constructors (1)

        public SpeechRecognitionViewModel()
        {
            initData();
            loadEnginesList();
        }

        #endregion Constructors

        #region Properties (2)

        public DelegateCommand<string> DoStopEngine { set; get; }

        public SpeechRecognitionModel SpeechRecognitionModelData { set; get; }

        #endregion Properties

        #region Methods (13)

        // Private Methods (13) 

        private bool canDoStopEngine(string data)
        {
            return SpeechRecognitionModelData.SelectedEngine != null &&
                 !string.IsNullOrWhiteSpace(SpeechRecognitionModelData.FileName);
        }

        void doStartSpeechRecognition(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            if (SpeechRecognitionModelData.SelectedEngine == null)
            {
                LogWindow.AddMessage(LogType.Error, "Please select an engine.");
                return;
            }

            startEngine(path);
            LogWindow.AddMessage(LogType.Info, "Engine has started.");
        }

        private RecognizerInfo getSelectedEngineRecognizerInfo()
        {
            if (SpeechRecognitionModelData.SelectedEngine == null) return null;
            return System.Speech.Recognition.SpeechRecognitionEngine.InstalledRecognizers().FirstOrDefault(x => x.Id == SpeechRecognitionModelData.SelectedEngine.Id);
        }

        private void initData()
        {
            SpeechRecognitionModelData = new SpeechRecognitionModel();
            SpeechRecognitionModelData.PropertyChanged += speechRecognitionModelDataPropertyChanged;
            App.Messenger.Register<string>("StartSpeechRecognition", doStartSpeechRecognition);
            DoStopEngine = new DelegateCommand<string>(data => { if (_sre != null) _sre.StopRecognition(); }, canDoStopEngine);
            App.Messenger.Register<string>("SpeechRecognitionFileChanged", speechRecognitionFileChanged);
        }

        private void initEngine(string filePath)
        {
            if (_sre != null) _sre.StopRecognition();
            _sre = new Sre(filePath, SpeechRecognitionModelData.SelectedEngine.Id)
            {
                RecognizeCompleted = message => LogWindow.AddMessage(LogType.Info, message),
                SpeechRecognized = subtitleItem => App.Messenger.NotifyColleagues("doAddVoiceSubtitle", subtitleItem),
                AudioPositionChanged = data => SpeechRecognitionModelData.AudioPosition = data,
                RecognizerAudioPositionChanged =
                    data => SpeechRecognitionModelData.RecognizerAudioPosition = data,
                Progress = data => SpeechRecognitionModelData.Progress = data,
                AverageConfidence = data => SpeechRecognitionModelData.Confidence = data
            };
            _sre.InitEngine();
        }

        private void load1StEngineAudioFormats(ReadOnlyCollection<RecognizerInfo> list)
        {
            if (list != null && list.Count == 1)
            {
                loadFormats(list[0]);
            }
        }

        private void loadEnginesList()
        {
            var list = System.Speech.Recognition.SpeechRecognitionEngine.InstalledRecognizers();
            SpeechRecognitionModelData.SpeechRecognitionEnginesData = new SpeechRecognitionEngines();
            foreach (var item in list)
                SpeechRecognitionModelData.SpeechRecognitionEnginesData.Add(
                    new Models.SpeechRecognitionEngine
                    {
                        Id = item.Id,
                        Name = item.Description
                    });
            load1StEngineAudioFormats(list);
        }

        private void loadFormats(RecognizerInfo engine)
        {
            if (engine == null) return;
            SpeechRecognitionModelData.AudioFormatsData = new AudioFormats();
            foreach (var item in engine.SupportedAudioFormats)
            {
                SpeechRecognitionModelData.AudioFormatsData.Add(
                    new AudioFormat
                        {
                            BitsPerSample = item.BitsPerSample,
                            BlockAlign = item.BlockAlign,
                            ChannelCount = item.ChannelCount,
                            Encodingformat = item.EncodingFormat.ToString(),
                            SamplesPerSecond = item.SamplesPerSecond
                        });
            }
        }

        private void resetGui()
        {
            SpeechRecognitionModelData.AudioLength = TimeSpan.Zero;
            SpeechRecognitionModelData.AudioPosition = TimeSpan.Zero;
            SpeechRecognitionModelData.Confidence = 0;
            SpeechRecognitionModelData.Progress = 0;
            SpeechRecognitionModelData.RecognizerAudioPosition = TimeSpan.Zero;
        }

        private void setMediaInfo()
        {
            SpeechRecognitionModelData.AudioLength = _sre.MediaLength;
            if (SpeechRecognitionModelData.SilenceTimeoutData == null)
            {
                SpeechRecognitionModelData.SilenceTimeoutData = _sre.InitialSilenceTimeouts;
            }
        }

        private void speechRecognitionFileChanged(string path)
        {
            SpeechRecognitionModelData.FileName = path;
            resetGui();
            initEngine(path);
            setMediaInfo();
        }

        void speechRecognitionModelDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedEngine":
                    loadFormats(getSelectedEngineRecognizerInfo());
                    break;
            }
        }

        private void startEngine(string filePath)
        {
            if (_sre == null) return;
            if (_sre.IsRunning)
            {
                LogWindow.AddMessage(LogType.Error, "Engine IsRunning.");
                return;
            }

            initEngine(filePath);
            _sre.StartRecognition(SpeechRecognitionModelData.SilenceTimeoutData);
        }

        #endregion Methods
    }
}