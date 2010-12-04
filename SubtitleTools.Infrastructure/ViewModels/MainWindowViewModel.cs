using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields (1)

        SubtitleItems _subtitleItemsDataInternal;

        #endregion Fields

        #region Constructors (1)

        public MainWindowViewModel()
        {
            setupItemsData();
            setupCommands();
            setupMessenger();
        }

        #endregion Constructors

        #region Properties (10)

        public DelegateCommand<string> DoConvertToUTF8 { set; get; }

        public DelegateCommand<string> DoDelete { set; get; }

        public DelegateCommand<string> DoJoinFiles { set; get; }

        public DelegateCommand<string> DoMerge { set; get; }

        public DelegateCommand<string> DoOpenFile { set; get; }

        public DelegateCommand<string> DoSaveChanges { set; get; }

        public DelegateCommand<string> DoSync { set; get; }

        public MainWindowGui MainWindowGuiData { set; get; }

        private SubtitleItems subtitleItemsDataInternal
        {
            set
            {
                _subtitleItemsDataInternal = value;
                if (SubtitleItemsDataView != null)
                {
                    SubtitleItemsDataView = CollectionViewSource.GetDefaultView(value);
                    raisePropertyChanged("SubtitleItemsDataView");
                }
            }
            get { return _subtitleItemsDataInternal; }
        }

        public ICollectionView SubtitleItemsDataView { set; get; }

        #endregion Properties

        #region Methods (25)

        // Private Methods (25) 

        bool canDoConvertToUTF8(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoDelete(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoJoinFiles(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoMerge(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        static bool canDoOpenFile(string file)
        {
            return true;
        }

        bool canDoSaveChanges(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoSync(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        void doCloseJoinPopup()
        {
            MainWindowGuiData.PopupDoJoinFilesIsOpen = false;
        }

        void doCloseSyncView()
        {
            MainWindowGuiData.PopupDoSyncIsOpen = false;
        }

        void doConvertToUTF8(string data)
        {
            try
            {
                MainWindowGuiData.IsBusy = true;
                var changer = new ChangeEncoding();
                if (changer.FixWindows1256(MainWindowGuiData.OpenedFilePath))
                {
                    setFlowDir(changer.IsRtl);
                    reBind();
                }
            }
            finally
            {
                MainWindowGuiData.IsBusy = false;
            }
        }

        void doDelete(string data)
        {
            DeleteRow.DeleteWholeRow(subtitleItemsDataInternal, MainWindowGuiData.SelectedItem, MainWindowGuiData.OpenedFilePath);
        }

        void doJoinFiles(string data)
        {
            MainWindowGuiData.PopupDoJoinFilesIsOpen = true;
        }

        void doMerge(string data)
        {
            try
            {
                MainWindowGuiData.IsBusy = true;
                Merge.StartInteractive(MainWindowGuiData.OpenedFilePath);
                reBind();
            }
            finally
            {
                MainWindowGuiData.IsBusy = false;
            }
        }

        void doOpenFile(string file)
        {
            try
            {
                MainWindowGuiData.IsBusy = true;
                var parser = new ParseSrt();
                subtitleItemsDataInternal = parser.ToObservableCollectionInteractive();
                setFlowDir(parser.IsRtl);
                MainWindowGuiData.OpenedFilePath = parser.FilePath;
                enableButtons();
                MainWindowGuiData.HeaderText = MainWindowGuiData.OpenedFilePath;
            }
            finally
            {
                MainWindowGuiData.IsBusy = false;
            }
        }

        void doRebindMsg()
        {
            try
            {
                MainWindowGuiData.IsBusy = true;
                reBind();
            }
            finally
            {
                MainWindowGuiData.IsBusy = false;
            }
        }

        void doSaveChanges(string data)
        {
            try
            {
                MainWindowGuiData.IsBusy = true;

                var newContent = ParseSrt.SubitemsToString(subtitleItemsDataInternal);
                File.WriteAllText(MainWindowGuiData.OpenedFilePath, newContent);
                LogWindow.AddMessage(LogType.Announcement, string.Format("Saved to:{0}", MainWindowGuiData.OpenedFilePath));
            }
            finally
            {
                MainWindowGuiData.IsBusy = false;
            }
        }

        private void doSearch(string data)
        {
            SubtitleItemsDataView.Filter = new Predicate<object>(obj =>
                {
                    var subtitleItem = obj as SubtitleItem;
                    if (obj == null) return false;
                    return subtitleItem != null && subtitleItem.Dialog.Contains(data);
                });
        }

        void doSync(string data)
        {
            MainWindowGuiData.PopupDoSyncIsOpen = true;
        }

        private void enableButtons()
        {
            DoConvertToUTF8.CanExecute(MainWindowGuiData.OpenedFilePath);
            DoSync.CanExecute(MainWindowGuiData.OpenedFilePath);
            DoMerge.CanExecute(MainWindowGuiData.OpenedFilePath);
            DoJoinFiles.CanExecute(MainWindowGuiData.OpenedFilePath);
            DoDelete.CanExecute(MainWindowGuiData.OpenedFilePath);
            DoSaveChanges.CanExecute(MainWindowGuiData.OpenedFilePath);
        }

        void mainWindowGuiDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchText")
            {
                doSearch(MainWindowGuiData.SearchText);
            }
        }

        private void reBind()
        {
            subtitleItemsDataInternal.Clear();
            subtitleItemsDataInternal = new ParseSrt().ToObservableCollectionFromFile(MainWindowGuiData.OpenedFilePath);
        }

        void setFlowDir(bool isRtl)
        {
            MainWindowGuiData.TableFlowDirection = isRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private void setupCommands()
        {
            DoOpenFile = new DelegateCommand<string>(doOpenFile, canDoOpenFile);
            DoConvertToUTF8 = new DelegateCommand<string>(doConvertToUTF8, canDoConvertToUTF8);
            DoSync = new DelegateCommand<string>(doSync, canDoSync);
            DoMerge = new DelegateCommand<string>(doMerge, canDoMerge);
            DoJoinFiles = new DelegateCommand<string>(doJoinFiles, canDoJoinFiles);
            DoDelete = new DelegateCommand<string>(doDelete, canDoDelete);
            DoSaveChanges = new DelegateCommand<string>(doSaveChanges, canDoSaveChanges);
        }

        private void setupItemsData()
        {
            MainWindowGuiData = new MainWindowGui();
            MainWindowGuiData.PropertyChanged += mainWindowGuiDataPropertyChanged;
            subtitleItemsDataInternal = new SubtitleItems();
            SubtitleItemsDataView = CollectionViewSource.GetDefaultView(subtitleItemsDataInternal);
        }

        private void setupMessenger()
        {
            App.Messenger.Register("reBind", doRebindMsg);
            App.Messenger.Register("doCloseJoinPopup", doCloseJoinPopup);
            App.Messenger.Register("doCloseSyncView", doCloseSyncView);
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
