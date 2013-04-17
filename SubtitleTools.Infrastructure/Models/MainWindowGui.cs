using System;
using System.Windows;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Infrastructure.Models
{
    public class MainWindowGui : ViewModelBase
    {
        #region Fields

        bool _translatorIsOpen;
        bool _doMergeIsEnabled;
        bool _doMixIsEnabled;
        string _headerText;
        bool _isBusy;
        bool _isLoaded;
        Uri _mediaFilePath = new Uri(@"MediaFile://");
        string _mergeFilePath;
        string _mixFilePath;
        string _openedFilePath;
        bool _popupDoDetectEncodingIsOpen;
        bool _popupDoJoinFilesIsOpen;
        bool _popupDoSyncIsOpen;
        int _scrollToIndex;
        string _searchText;
        SubtitleItem _selectedItem;
        FlowDirection _tableFlowDirection = FlowDirection.LeftToRight;
        string _wavFilePath;
        string[] _wavFilesPath;

        #endregion Fields

        #region Properties

        public bool DoMergeIsEnabled
        {
            get { return _doMergeIsEnabled; }
            set
            {
                if (_doMergeIsEnabled == value) return;
                _doMergeIsEnabled = value;
                RaisePropertyChanged("DoMergeIsEnabled");
            }
        }

        public bool DoMixIsEnabled
        {
            get { return _doMixIsEnabled; }
            set
            {
                if (_doMixIsEnabled == value) return;
                _doMixIsEnabled = value;
                RaisePropertyChanged("DoMixIsEnabled");
            }
        }

        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                if (_headerText == value) return;
                _headerText = value;
                RaisePropertyChanged("HeaderText");
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                RaisePropertyChanged("IsLoaded");
            }
        }

        //just a not null value
        public Uri MediaFilePath
        {
            get { return _mediaFilePath; }
            set
            {
                if (_mediaFilePath == value) return;
                _mediaFilePath = value;
                RaisePropertyChanged("MediaFilePath");
            }
        }

        public string MergeFilePath
        {
            get { return _mergeFilePath; }
            set
            {
                if (_mergeFilePath == value) return;
                _mergeFilePath = value;
                RaisePropertyChanged("MergeFilePath");
            }
        }

        public string MixFilePath
        {
            get { return _mixFilePath; }
            set
            {
                if (_mixFilePath == value) return;
                _mixFilePath = value;
                RaisePropertyChanged("MixFilePath");
            }
        }

        public string OpenedFilePath
        {
            get { return _openedFilePath; }
            set
            {
                if (_openedFilePath == value) return;
                _openedFilePath = value;
                RaisePropertyChanged("OpenedFilePath");
            }
        }

        public bool PopupDoDetectEncodingIsOpen
        {
            get { return _popupDoDetectEncodingIsOpen; }
            set
            {
                if (_popupDoDetectEncodingIsOpen == value) return;
                _popupDoDetectEncodingIsOpen = value;
                RaisePropertyChanged("PopupDoDetectEncodingIsOpen");
            }
        }

        public bool PopupDoJoinFilesIsOpen
        {
            get { return _popupDoJoinFilesIsOpen; }
            set
            {
                if (_popupDoJoinFilesIsOpen == value) return;
                _popupDoJoinFilesIsOpen = value;
                RaisePropertyChanged("PopupDoJoinFilesIsOpen");
            }
        }

        public bool PopupDoSyncIsOpen
        {
            get { return _popupDoSyncIsOpen; }
            set
            {
                if (_popupDoSyncIsOpen == value) return;
                _popupDoSyncIsOpen = value;
                RaisePropertyChanged("PopupDoSyncIsOpen");
            }
        }

        public int ScrollToIndex
        {
            get { return _scrollToIndex; }
            set
            {
                if (_scrollToIndex == value) return;
                _scrollToIndex = value;
                RaisePropertyChanged("ScrollToIndex");
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                RaisePropertyChanged("SearchText");
            }
        }

        public SubtitleItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        public FlowDirection TableFlowDirection
        {
            get { return _tableFlowDirection; }
            set
            {
                if (_tableFlowDirection == value) return;
                _tableFlowDirection = value;
                RaisePropertyChanged("TableFlowDirection");
            }
        }

        public string WavFilePath
        {
            get { return _wavFilePath; }
            set
            {
                _wavFilePath = value;
                RaisePropertyChanged("WavFilePath");
            }
        }

        public string[] WavFilesPath
        {
            get { return _wavFilesPath; }
            set
            {
                _wavFilesPath = value;
                RaisePropertyChanged("WavFilesPath");
            }
        }

        public bool TranslatorIsOpen
        {
            get { return _translatorIsOpen; }
            set
            {
                if (_translatorIsOpen == value) return;
                _translatorIsOpen = value;
                RaisePropertyChanged("TranslatorIsOpen");
            }
        }

        #endregion Properties
    }
}
