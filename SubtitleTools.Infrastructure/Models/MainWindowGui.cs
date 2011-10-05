using System;
using System.ComponentModel;
using System.Windows;

namespace SubtitleTools.Infrastructure.Models
{
    public class MainWindowGui : INotifyPropertyChanged
    {
        #region Fields (16)

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

        #endregion Fields

        #region Properties (16)

        public bool DoMergeIsEnabled
        {
            get { return _doMergeIsEnabled; }
            set
            {
                if (_doMergeIsEnabled == value) return;
                _doMergeIsEnabled = value;
                raisePropertyChanged("DoMergeIsEnabled");
            }
        }

        public bool DoMixIsEnabled
        {
            get { return _doMixIsEnabled; }
            set
            {
                if (_doMixIsEnabled == value) return;
                _doMixIsEnabled = value;
                raisePropertyChanged("DoMixIsEnabled");
            }
        }

        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                if (_headerText == value) return;
                _headerText = value;
                raisePropertyChanged("HeaderText");
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                raisePropertyChanged("IsBusy");
            }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                raisePropertyChanged("IsLoaded");
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
                raisePropertyChanged("MediaFilePath");
            }
        }

        public string MergeFilePath
        {
            get { return _mergeFilePath; }
            set
            {
                if (_mergeFilePath == value) return;
                _mergeFilePath = value;
                raisePropertyChanged("MergeFilePath");
            }
        }

        public string MixFilePath
        {
            get { return _mixFilePath; }
            set
            {
                if (_mixFilePath == value) return;
                _mixFilePath = value;
                raisePropertyChanged("MixFilePath");
            }
        }

        public string OpenedFilePath
        {
            get { return _openedFilePath; }
            set
            {
                if (_openedFilePath == value) return;
                _openedFilePath = value;
                raisePropertyChanged("OpenedFilePath");
            }
        }

        public bool PopupDoDetectEncodingIsOpen
        {
            get { return _popupDoDetectEncodingIsOpen; }
            set
            {
                if (_popupDoDetectEncodingIsOpen == value) return;
                _popupDoDetectEncodingIsOpen = value;
                raisePropertyChanged("PopupDoDetectEncodingIsOpen");
            }
        }

        public bool PopupDoJoinFilesIsOpen
        {
            get { return _popupDoJoinFilesIsOpen; }
            set
            {
                if (_popupDoJoinFilesIsOpen == value) return;
                _popupDoJoinFilesIsOpen = value;
                raisePropertyChanged("PopupDoJoinFilesIsOpen");
            }
        }

        public bool PopupDoSyncIsOpen
        {
            get { return _popupDoSyncIsOpen; }
            set
            {
                if (_popupDoSyncIsOpen == value) return;
                _popupDoSyncIsOpen = value;
                raisePropertyChanged("PopupDoSyncIsOpen");
            }
        }

        public int ScrollToIndex
        {
            get { return _scrollToIndex; }
            set
            {
                if (_scrollToIndex == value) return;
                _scrollToIndex = value;
                raisePropertyChanged("ScrollToIndex");
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                raisePropertyChanged("SearchText");
            }
        }

        public SubtitleItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                raisePropertyChanged("SelectedItem");
            }
        }

        public FlowDirection TableFlowDirection
        {
            get { return _tableFlowDirection; }
            set
            {
                if (_tableFlowDirection == value) return;
                _tableFlowDirection = value;
                raisePropertyChanged("TableFlowDirection");
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
