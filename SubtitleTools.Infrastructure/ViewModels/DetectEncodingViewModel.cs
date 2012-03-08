using SubtitleTools.Common.EncodingHelper;
using SubtitleTools.Common.EncodingHelper.Model;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Core;
using System.IO;
using System.Threading;
using System;
using SubtitleTools.Common.Logger;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class DetectEncodingViewModel : ViewModelBase
    {
        #region Fields (5)

        int _addIndex;
        EncodingsInf _encodingsInfData;
        string _filePath;
        string _folderPath;
        EncodingInf _selectedEncoding;

        #endregion Fields

        #region Constructors (1)

        public DetectEncodingViewModel()
        {
            EncodingsInfData = new EncodingsInf();
            setupCommands();
        }

        #endregion Constructors

        #region Properties (7)

        public int AddIndex
        {
            set
            {
                if (_addIndex == value) return;
                _addIndex = value;
                RaisePropertyChanged("AddIndex");
            }
            get { return _addIndex; }
        }

        public DelegateCommand<string> DoCloseConvertEncodingView { set; get; }

        public DelegateCommand<string> DoConvertEncoding { set; get; }

        public EncodingsInf EncodingsInfData
        {
            set
            {
                _encodingsInfData = value;
                RaisePropertyChanged("EncodingsInfData");
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
                RaisePropertyChanged("FilePath");
                EncodingsInfData = DetectEncoding.DetectProbableFileCodepages(value);
                FolderPath = Path.GetDirectoryName(value);
            }
        }

        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                if (_folderPath == value) return;
                _folderPath = value;
                RaisePropertyChanged("FolderPath");
            }
        }

        public EncodingInf SelectedEncoding
        {
            get { return _selectedEncoding; }
            set
            {
                if (value == null) return;
                _selectedEncoding = value;
                RaisePropertyChanged("SelectedEncoding");
            }
        }

        #endregion Properties

        #region Methods (7)

        // Private Methods (7) 

        static bool canDoCloseConvertEncodingView(string data)
        {
            return true;
        }

        bool canDoConvertEncoding(string filePath)
        {
            return SelectedEncoding != null;
        }

        private void convertThisFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            if (new ChangeEncoding().ToUTF8(filePath, SelectedEncoding.BodyName))
            {
                App.Messenger.NotifyColleagues("reBind");
            }
        }

        private void convertThisFolder()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                LogWindow.AddMessage(LogType.Alert, "Please select a folder.");
                return;
            }
            new Thread(() =>
                {
                    try
                    {
                        new ChangeEncoding().ConvertAllFilesEncodings(FolderPath, SelectedEncoding.BodyName);
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.LogExceptionToFile(ex);
                        LogWindow.AddMessage(LogType.Error, ex.Message);
                    }
                }).Start();
        }

        static void doCloseConvertEncodingView(string data)
        {
            App.Messenger.NotifyColleagues("doCloseConvertEncodingView");
        }

        void doConvertEncoding(string filePath)
        {
            if (AddIndex == -1)
            {
                LogWindow.AddMessage(LogType.Alert, "Please select one of the items.");
                return;
            }

            if (AddIndex == 0)
            {
                convertThisFile(filePath);
            }
            else
            {
                convertThisFolder();
            }
        }

        private void setupCommands()
        {
            DoConvertEncoding = new DelegateCommand<string>(doConvertEncoding, canDoConvertEncoding);
            DoCloseConvertEncodingView = new DelegateCommand<string>(doCloseConvertEncodingView, canDoCloseConvertEncodingView);
        }

        #endregion Methods
    }
}
