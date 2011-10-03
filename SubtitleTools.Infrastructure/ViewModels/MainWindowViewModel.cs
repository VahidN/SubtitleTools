using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using SubtitleTools.Common.Logger;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Common.Regex;
using SubtitleTools.Common.Threading;
using SubtitleTools.Common.Toolkit;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Models;

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
                    App.Messenger.NotifyColleagues("SubtitleItems", _subtitleItemsDataInternal);
                }
            }
            get { return _subtitleItemsDataInternal; }
        }

        public ICollectionView SubtitleItemsDataView { set; get; }

        #endregion Properties

        #region Methods (33)

        // Private Methods (33) 

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

        void doAddSubtitle(SubtitleItem subtitleItem)
        {
            if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath))
            {
                LogWindow.AddMessage(LogType.Error, "Please open an empty subtitle file first.");
                return;
            }

            var localSubItems = new List<SubtitleItem>();
            foreach (var item in subtitleItemsDataInternal)
                localSubItems.Add(item);

            var newItem = new SubtitleItem
            {
                Dialog = subtitleItem.Dialog,
                EndTs = subtitleItem.EndTs,
                StartTs = subtitleItem.StartTs
            };

            localSubItems.Add(newItem);
            localSubItems = localSubItems.OrderBy(x => x.StartTs).ToList();

            int i = 1;
            var finalItems = new SubtitleItems();
            foreach (var item in localSubItems)
            {
                item.Number = i++;
                finalItems.Add(item);
            }

            subtitleItemsDataInternal = finalItems;
            setFlowDir(subtitleItem.Dialog.ContainsFarsi());
            saveToFile();
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
            Task.Factory.StartNew(() => doSaveChanges("All"));
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

        void doMix()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath) ||
                    string.IsNullOrWhiteSpace(MainWindowGuiData.MixFilePath))
                {
                    return;
                }

                MainWindowGuiData.IsBusy = true;
                MixFiles.WriteMixedList(mainFilePath: MainWindowGuiData.OpenedFilePath,
                                       fromFilepath: MainWindowGuiData.MixFilePath);
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
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
            finally
            {
                DispatcherHelper.DispatchAction(enableButtons);
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

                if (data == "All")
                {
                    var localSubItems = new SubtitleItems();
                    foreach (var item in subtitleItemsDataInternal)
                        localSubItems.Add(item);

                    foreach (var row in localSubItems)
                    {
                        row.Dialog = row.Dialog.InsertRle();
                    }

                    subtitleItemsDataInternal = localSubItems;
                }
                else
                {
                    //modify changed rows
                    foreach (var row in _changedRows)
                    {
                        subtitleItemsDataInternal[row].Dialog
                            = subtitleItemsDataInternal[row].Dialog.InsertRle();
                    }
                }

                saveToFile();
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

        void doScrollToIndex(int idx)
        {
            MainWindowGuiData.ScrollToIndex = idx;
        }

        private void doSearch(string data)
        {
            SubtitleItemsDataView.Filter = obj =>
                {
                    if (obj == null) return false;
                    var subtitleItem = obj as SubtitleItem;
                    return subtitleItem != null && subtitleItem.Dialog.ToLower().Contains(data.ToLower());
                };
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
            MainWindowGuiData.DoMixIsEnabled = !string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath);
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
                case "MixFilePath":
                    new Thread(doMix).Start();
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

        private void saveToFile()
        {
            var newContent = ParseSrt.SubitemsToString(subtitleItemsDataInternal);
            File.WriteAllText(MainWindowGuiData.OpenedFilePath, newContent.ApplyUnifiedYeKe());
            LogWindow.AddMessage(LogType.Announcement, string.Format("Saved to:{0}", MainWindowGuiData.OpenedFilePath));
            setFlowDir(newContent.ContainsFarsi());
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
            App.Messenger.Register<SubtitleItem>("doAddSubtitle", doAddSubtitle);
            App.Messenger.Register<int>("doScrollToIndex", doScrollToIndex);
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
