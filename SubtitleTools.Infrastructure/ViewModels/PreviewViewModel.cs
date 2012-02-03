using System;
using System.Threading.Tasks;
using System.Windows;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Common.Regex;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class PreviewViewModel
    {
        #region Fields (3)

        string _lastDialog;
        bool _seekBarMaximumIsSet;
        SubtitleItems _subtitleItems;

        #endregion Fields

        #region Constructors (1)

        public PreviewViewModel()
        {
            initLocalItems();
            initCommands();
            initMessenger();
        }

        #endregion Constructors

        #region Properties (7)

        public DelegateCommand<string> DoAddSubtitle { set; get; }

        public DelegateCommand<string> DoClearSubtitle { set; get; }

        public DelegateCommand<string> DoPause { set; get; }

        public DelegateCommand<string> DoPlay { set; get; }

        public DelegateCommand<string> DoStop { set; get; }

        public PreviewModel PreviewModelData { set; get; }

        public SubtitleItem SubtitleItemData { set; get; }

        #endregion Properties

        #region Methods (22)

        // Private Methods (22) 

        private void clearCtrls()
        {
            SubtitleItemData.Dialog = string.Empty;
            SubtitleItemData.StartTs = TimeSpan.Zero;
            SubtitleItemData.EndTs = TimeSpan.Zero;
        }

        void doAddSubtitle(string data)
        {
            SubtitleItemData.Dialog = SubtitleItemData.Dialog.InsertRle();
            App.Messenger.NotifyColleagues("doAddSubtitle", SubtitleItemData);
        }

        void doClearSubtitle(string data)
        {
            clearCtrls();
        }

        void doPause(string data)
        {
            _seekBarMaximumIsSet = false;
            PreviewModelData.PlayMedia = false;
            setPlayImage();
            setSubtitleEnd();
        }

        void doPlay(string data)
        {
            _seekBarMaximumIsSet = false;
            PreviewModelData.PlayMedia = !PreviewModelData.PlayMedia;
            setPlayImage();
            setSubtitleStart();
            setSubtitleEnd();
        }

        void doStop(string data)
        {
            PreviewModelData.StopMedia = !PreviewModelData.StopMedia;
            PreviewModelData.PlayImage = "play";
            _seekBarMaximumIsSet = false;
        }

        private void initCommands()
        {
            DoStop = new DelegateCommand<string>(doStop);
            DoPlay = new DelegateCommand<string>(doPlay);
            DoPause = new DelegateCommand<string>(doPause);
            DoAddSubtitle = new DelegateCommand<string>(doAddSubtitle);
            DoClearSubtitle = new DelegateCommand<string>(doClearSubtitle);
        }

        private void initLocalItems()
        {
            _subtitleItems = new SubtitleItems();
            PreviewModelData = new PreviewModel();
            SubtitleItemData = new SubtitleItem();

            PreviewModelData.PlayImage = "play";
            PreviewModelData.DragCompleted = true;
            PreviewModelData.PropertyChanged += previewModelDataPropertyChanged;
            SubtitleItemData.PropertyChanged += subtitleItemDataPropertyChanged;
            SubtitleItemData.CaretIndex = 1;
        }

        private void initMessenger()
        {
            App.Messenger.Register<SubtitleItems>("SubtitleItems", data => { _subtitleItems = data; });
            App.Messenger.Register<TimeSpan>("doSetMediaPosition", data =>
            {
                if (PreviewModelData.PlayMedia) return;
                PreviewModelData.MediaManualPosition = data;
            });
            App.Messenger.Register("doClearSubtitle", () => doClearSubtitle(string.Empty));
        }

        void previewModelDataPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                switch (e.PropertyName)
                {
                    case "MediaNaturalDuration":
                        setSeekBarMaximum();
                        break;
                    case "SeekBarValue":
                        break;
                    case "PlayMedia":
                        setPlayImage();
                        setMediaPosition();
                        break;
                    case "MediaPosition":
                        setMediaPosition();
                        showDialog();
                        break;
                    case "DragCompleted":
                        setSeekBarMediaPosition();
                        break;
                    case "MediaError":
                        showError();
                        break;
                }
            });
        }

        void setFlowDir()
        {
            SubtitleItemData.DialogFlowDirection = SubtitleItemData.Dialog.ContainsFarsi() ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private void setMaxWordsPerLine()
        {
            var originalValue = SubtitleItemData.Dialog.Trim();
            var newValue = ParseSrt.SetMaxWordsPerLine(SubtitleItemData.Dialog);
            if (originalValue != newValue)
            {
                SubtitleItemData.Dialog = newValue;
                SubtitleItemData.CaretIndex = newValue.Length;
            }
        }

        private void setMediaPosition()
        {
            PreviewModelData.SeekBarValue = PreviewModelData.MediaPosition.TotalSeconds;
            PreviewModelData.MediaLen = timeSpanToString(PreviewModelData.MediaPosition);
        }

        private void setPlayImage()
        {
            PreviewModelData.PlayImage = PreviewModelData.PlayMedia ? "pause" : "play";
        }

        private void setSeekBarMaximum()
        {
            if (_seekBarMaximumIsSet) return;

            var ts = PreviewModelData.MediaNaturalDuration;
            PreviewModelData.SeekBarMaximum = ts.TotalSeconds;
            PreviewModelData.SeekBarLargeChange = Math.Min(10, ts.Seconds / 10);
            _seekBarMaximumIsSet = true;
        }

        private void setSeekBarMediaPosition()
        {
            PreviewModelData.MediaManualPosition = TimeSpan.FromSeconds(PreviewModelData.SeekBarValue);
        }

        private void setSubtitleEnd()
        {
            if (!PreviewModelData.PlayMedia)
            {
                SubtitleItemData.EndTs = PreviewModelData.MediaPosition;
            }
        }

        private void setSubtitleStart()
        {
            if (PreviewModelData.PlayMedia)
            {
                SubtitleItemData.StartTs = PreviewModelData.MediaPosition;
            }
        }

        private void showDialog()
        {
            var subtitle = ParseSrt.GetCurrentSubtile(_subtitleItems, PreviewModelData.MediaPosition);
            if (subtitle == null)
            {
                PreviewModelData.Dialog = string.Empty;
                return;
            }

            if (_lastDialog == subtitle.Dialog) return;

            PreviewModelData.Dialog = subtitle.Dialog;
            _lastDialog = PreviewModelData.Dialog;
            PreviewModelData.DialogFlowDirection =
                subtitle.Dialog.ContainsFarsi() ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            App.Messenger.NotifyColleagues("doScrollToIndex", subtitle.Number - 1);
        }

        private void showError()
        {
            LogWindow.AddMessage(LogType.Error, PreviewModelData.MediaError);
        }

        void subtitleItemDataPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Dialog":
                    setMaxWordsPerLine();
                    setFlowDir();
                    break;
            }
        }

        static string timeSpanToString(TimeSpan lineTs)
        {
            return string.Format("{0}:{1}:{2},{3}", lineTs.Hours.ToString("D2"), lineTs.Minutes.ToString("D2"), lineTs.Seconds.ToString("D2"), lineTs.Milliseconds.ToString("D3"));
        }

        #endregion Methods
    }
}
