using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using SubtitleTools.Common.Logger;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Common.Threading;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Models;
using System.Collections.Specialized;
using System.Collections.Generic;
using SubtitleTools.Common.Toolkit;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields (2)

        SortedSet<int> _changedRows;
        SubtitleItems _subtitleItemsDataInternal;

        #endregion Fields

        #region Constructors (1)

        public MainWindowViewModel()
        {
            setupItemsData();
            setupCommands();
            setupMessenger();
            processCommandLineArguments();
        }

        #endregion Constructors

        #region Properties (9)

        public DelegateCommand<string> DoConvertToUTF8 { set; get; }

        public DelegateCommand<string> DoDelete { set; get; }

        public DelegateCommand<string> DoInsertRLE { set; get; }

        public DelegateCommand<string> DoJoinFiles { set; get; }

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
                    _changedRows = new SortedSet<int>();
                    SubtitleItemsDataView = CollectionViewSource.GetDefaultView(value);
                    raisePropertyChanged("SubtitleItemsDataView");
                }
            }
            get { return _subtitleItemsDataInternal; }
        }

        public ICollectionView SubtitleItemsDataView { set; get; }

        #endregion Properties

        #region Methods (29)

        // Private Methods (29) 

        bool canDoConvertToUTF8(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoDelete(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoInsertRLE(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoJoinFiles(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoSaveChanges(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        bool canDoSync(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        private void deleteRow()
        {
            try
            {
                MainWindowGuiData.IsBusy = true;

                var localSubItems = new SubtitleItems();
                foreach (var item in subtitleItemsDataInternal)
                    localSubItems.Add(item);

                DeleteRow.DeleteWholeRow(
                    localSubItems,
                    MainWindowGuiData.SelectedItem,
                    MainWindowGuiData.OpenedFilePath
                    );

                subtitleItemsDataInternal = localSubItems;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
            finally
            {
                MainWindowGuiData.IsBusy = false;
            }
        }

        void doCloseConvertEncodingView()
        {
            MainWindowGuiData.PopupDoDetectEncodingIsOpen = false;
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
            MainWindowGuiData.PopupDoDetectEncodingIsOpen = true;
        }

        void doDelete(string data)
        {
            new Thread(deleteRow).Start();
        }

        void doInsertRLE(string data)
        {
            if (MainWindowGuiData.SelectedItem == null)
            {
                LogWindow.AddMessage(LogType.Alert, "Please select a row.");
                return;
            }

            MainWindowGuiData.SelectedItem.Dialog =
                UnicodeRle.InsertBefore(MainWindowGuiData.SelectedItem.Dialog);

            doSaveChanges(string.Empty);
        }

        void doJoinFiles(string data)
        {
            MainWindowGuiData.PopupDoJoinFilesIsOpen = true;
        }

        void doMerge()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath) ||
                    string.IsNullOrWhiteSpace(MainWindowGuiData.MergeFilePath))
                {
                    return;
                }

                MainWindowGuiData.IsBusy = true;
                Merge.WriteMergedList(mainFilePath: MainWindowGuiData.OpenedFilePath,
                                      fromFilepath: MainWindowGuiData.MergeFilePath);
                reBind();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
            finally
            {
                MainWindowGuiData.IsBusy = false;
            }
        }

        void doOpenFile()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath)) return;

                MainWindowGuiData.IsBusy = true;

                var parser = new ParseSrt();
                subtitleItemsDataInternal = parser.ToObservableCollectionFromFile(MainWindowGuiData.OpenedFilePath);
                MainWindowGuiData.HeaderText = MainWindowGuiData.OpenedFilePath;

                setFlowDir(parser.IsRtl);

                DispatcherHelper.DispatchAction(enableButtons);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
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

                //modify changed rows
                foreach (var row in _changedRows)
                {
                    subtitleItemsDataInternal[row].Dialog
                        = UnicodeRle.InsertBefore(subtitleItemsDataInternal[row].Dialog);
                }

                var newContent = ParseSrt.SubitemsToString(subtitleItemsDataInternal);
                File.WriteAllText(MainWindowGuiData.OpenedFilePath, newContent.ApplyUnifiedYeKe());
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
            MainWindowGuiData.DoMergeIsEnabled = !string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath);
            DoJoinFiles.CanExecute(MainWindowGuiData.OpenedFilePath);
            DoDelete.CanExecute(MainWindowGuiData.OpenedFilePath);
            DoSaveChanges.CanExecute(MainWindowGuiData.OpenedFilePath);
            DoInsertRLE.CanExecute(MainWindowGuiData.OpenedFilePath);
        }

        void mainWindowGuiDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SearchText":
                    doSearch(MainWindowGuiData.SearchText.ApplyUnifiedYeKe());
                    break;
                case "OpenedFilePath":
                    new Thread(doOpenFile).Start();
                    break;
                case "MergeFilePath":
                    new Thread(doMerge).Start();
                    break;
            }
        }

        private void processCommandLineArguments()
        {
            var startupFileName = Application.Current.Properties["StartupFileName"];
            if (startupFileName == null) return;
            if (string.IsNullOrEmpty(startupFileName.ToString())) return;
            if (!File.Exists(startupFileName.ToString())) return;
            MainWindowGuiData.OpenedFilePath = startupFileName.ToString();
            new Thread(doOpenFile).Start();
        }

        private void reBind()
        {
            new Thread(doOpenFile).Start();
        }

        void setFlowDir(bool isRtl)
        {
            MainWindowGuiData.TableFlowDirection = isRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private void setupCommands()
        {
            DoConvertToUTF8 = new DelegateCommand<string>(doConvertToUTF8, canDoConvertToUTF8);
            DoSync = new DelegateCommand<string>(doSync, canDoSync);
            DoJoinFiles = new DelegateCommand<string>(doJoinFiles, canDoJoinFiles);
            DoDelete = new DelegateCommand<string>(doDelete, canDoDelete);
            DoSaveChanges = new DelegateCommand<string>(doSaveChanges, canDoSaveChanges);
            DoInsertRLE = new DelegateCommand<string>(doInsertRLE, canDoInsertRLE);
        }

        private void setupItemsData()
        {
            _changedRows = new SortedSet<int>();
            MainWindowGuiData = new MainWindowGui();
            MainWindowGuiData.PropertyChanged += mainWindowGuiDataPropertyChanged;
            subtitleItemsDataInternal = new SubtitleItems();
            subtitleItemsDataInternal.CollectionChanged += subtitleItemsDataInternalCollectionChanged;
            SubtitleItemsDataView = CollectionViewSource.GetDefaultView(subtitleItemsDataInternal);
        }

        private void setupMessenger()
        {
            App.Messenger.Register("reBind", doRebindMsg);
            App.Messenger.Register("doCloseJoinPopup", doCloseJoinPopup);
            App.Messenger.Register("doCloseSyncView", doCloseSyncView);
            App.Messenger.Register("doCloseConvertEncodingView", doCloseConvertEncodingView);
        }

        void subtitleItemsDataInternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove) return;
            foreach (SubtitleItem item in e.NewItems)
            {
                _changedRows.Add(item.Number);
            }
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
