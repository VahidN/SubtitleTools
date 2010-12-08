using System.ComponentModel;
using SubtitleTools.Common.EncodingHelper;
using SubtitleTools.Common.EncodingHelper.Model;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Core;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class DetectEncodingViewModel : INotifyPropertyChanged
    {
        #region Fields (3)

        EncodingsInf _encodingsInfData;
        string _filePath;
        EncodingInf _selectedEncoding;

        #endregion Fields

        #region Constructors (1)

        public DetectEncodingViewModel()
        {
            EncodingsInfData = new EncodingsInf();
            setupCommands();
        }

        #endregion Constructors

        #region Properties (5)

        public DelegateCommand<string> DoCloseConvertEncodingView { set; get; }

        public DelegateCommand<string> DoConvertEncoding { set; get; }

        public EncodingsInf EncodingsInfData
        {
            set
            {
                _encodingsInfData = value;
                raisePropertyChanged("EncodingsInfData");
            }
            get { return _encodingsInfData; }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;
                if (_filePath == value) return;
                _filePath = value;
                raisePropertyChanged("FilePath");
                EncodingsInfData = DetectEncoding.DetectProbableFileCodepages(value);
            }
        }

        public EncodingInf SelectedEncoding
        {
            get { return _selectedEncoding; }
            set
            {
                if (value == null) return;
                _selectedEncoding = value;
                raisePropertyChanged("SelectedEncoding");
                enableButton();
            }
        }

        #endregion Properties

        #region Methods (6)

        // Private Methods (6) 

        static bool canDoCloseConvertEncodingView(string data)
        {
            return true;
        }

        bool canDoConvertEncoding(string filePath)
        {
            return SelectedEncoding != null;
        }

        static void doCloseConvertEncodingView(string data)
        {
            App.Messenger.NotifyColleagues("doCloseConvertEncodingView");
        }

        void doConvertEncoding(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            if (new ChangeEncoding().ToUTF8(filePath, SelectedEncoding.BodyName))
            {
                App.Messenger.NotifyColleagues("reBind");
            }
        }

        private void enableButton()
        {
            if (SelectedEncoding == null) return;
            DoConvertEncoding.CanExecute(SelectedEncoding.BodyName);
        }

        private void setupCommands()
        {
            DoConvertEncoding = new DelegateCommand<string>(doConvertEncoding, canDoConvertEncoding);
            DoCloseConvertEncodingView = new DelegateCommand<string>(doCloseConvertEncodingView, canDoCloseConvertEncodingView);
        }

        #endregion Methods

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
