using System.ComponentModel;
using System.Windows;

namespace SubtitleTools.Infrastructure.Models
{
    public class MainWindowGui : INotifyPropertyChanged
    {
        #region Fields (11)

        bool _doMergeIsEnabled;
        string _headerText;
        bool _isBusy;
        string _mergeFilePath;
        string _openedFilePath;
        bool _popupDoDetectEncodingIsOpen;
        bool _popupDoJoinFilesIsOpen;
        bool _popupDoSyncIsOpen;
        string _searchText;
        SubtitleItem _selectedItem;
        FlowDirection _tableFlowDirection = FlowDirection.LeftToRight;

        #endregion Fields

        #region Properties (11)

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
