using System.ComponentModel;
using System.Windows;

namespace SubtitleTools.Infrastructure.Models
{
    public class MainWindowGui : INotifyPropertyChanged
    {
        #region Fields (7)

        string _headerText;
        bool _isBusy;
        string _openedFilePath;
        string _searchText;
        bool _popupDoJoinFilesIsOpen;
        bool _popupDoSyncIsOpen;
        SubtitleItem _selectedItem;
        FlowDirection _tableFlowDirection = FlowDirection.LeftToRight;

        #endregion Fields

        #region Properties (7)

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
