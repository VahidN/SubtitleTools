using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields (3)

        SortedSet<int> _changedRows;
        TimeSpan _lastStartTs;
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

        #region Properties (11)

        public DelegateCommand<string> DoConvertToUTF8 { set; get; }

        public DelegateCommand<string> DoDelete { set; get; }

        public DelegateCommand<string> DoInsertRLE { set; get; }

        public DelegateCommand<string> DoJoinFiles { set; get; }

        public DelegateCommand<string> DoRecalculate { set; get; }

        public DelegateCommand<string> DoSaveChanges { set; get; }

        public DelegateCommand<string> DoStartSpeechRecognition { set; get; }

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
                    RaisePropertyChanged("SubtitleItemsDataView");
                    App.Messenger.NotifyColleagues("SubtitleItems", _subtitleItemsDataInternal);
                    Task.Factory.StartNew(showConflicts);
                }
            }
            get { return _subtitleItemsDataInternal; }
        }

        public ICollectionView SubtitleItemsDataView { set; get; }

        #endregion Properties

        #region Methods (43)

        // Private Methods (43) 

        private void addSubtitleToFile(SubtitleItem subtitleItem, string mediaPath)
        {
            if (string.IsNullOrWhiteSpace(subtitleItem.Dialog)) return;

            var subtitleFilePath = string.Empty;
            if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath))
            {
                if (!string.IsNullOrWhiteSpace(mediaPath))
                {
                    subtitleFilePath = createEmptySubtitleFile(mediaPath);
                }
                else
                {
                    LogWindow.AddMessage(LogType.Error, "Please open an empty subtitle file first.");
                    return;
                }
            }

            subtitleItemsDataInternal = ParseSrt.AddSubtitleItemToList(subtitleItemsDataInternal, subtitleItem);
            setFlowDir(subtitleItem.Dialog.ContainsFarsi());
            saveToFile(subtitleFilePath);
            _lastStartTs = subtitleItem.StartTs;
            doScrollToIndex(subtitleItemsDataInternal.Count - 1);
            if (!string.IsNullOrEmpty(subtitleFilePath)) MainWindowGuiData.OpenedFilePath = subtitleFilePath;
            App.Messenger.NotifyColleagues("doClearSubtitle");
        }

        private void backupSubtitleFile()
        {
            if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath)) return;
            var backupFile = string.Format("{0}-{1}.bak", MainWindowGuiData.OpenedFilePath, Guid.NewGuid().ToString());
            File.Copy(MainWindowGuiData.OpenedFilePath, backupFile, overwrite: true);
            File.WriteAllText(MainWindowGuiData.OpenedFilePath, string.Empty);
            openSubtitleFile();
        }

        bool canDoStartSpeechRecognition(string data)
        {
            return !string.IsNullOrWhiteSpace(MainWindowGuiData.WavFilePath) &&
                    System.Speech.Recognition.SpeechRecognitionEngine.InstalledRecognizers().Any();
        }

        private void clearSrtPanel()
        {
            subtitleItemsDataInternal = new SubtitleItems();
        }

        private string createEmptySubtitleFile(string mediaPath)
        {
            var subtitleFilePath = Path.ChangeExtension(mediaPath, ".srt");
            if (!File.Exists(subtitleFilePath))
            {
                File.WriteAllText(subtitleFilePath, string.Empty, Encoding.UTF8);
                LogWindow.AddMessage(LogType.Info, "An empty subtitle file @" + subtitleFilePath + " has been created.");
            }
            return subtitleFilePath;
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
            addSubtitleToFile(subtitleItem, MainWindowGuiData.MediaFilePath.LocalPath);
        }

        void doAddVoiceSubtitle(SubtitleItem subtitleItem)
        {
            addSubtitleToFile(subtitleItem, MainWindowGuiData.WavFilePath);
        }

        void doChangeWavFilePath(string path)
        {
            MainWindowGuiData.WavFilePath = path;
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
                if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath))
                {
                    clearSrtPanel();
                    return;
                }

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

        void doRecalculate(string data)
        {
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath)) return;
                        MainWindowGuiData.IsBusy = true;
                        Sync.RecalculateRowNumbers(subtitleItemsDataInternal, MainWindowGuiData.OpenedFilePath);
                        showConflicts();
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
                });
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

        private void doSelectedItem()
        {
            if (MainWindowGuiData.SelectedItem == null) return;
            var startTs = MainWindowGuiData.SelectedItem.StartTs;
            if (_lastStartTs == startTs) return;
            App.Messenger.NotifyColleagues("doSetMediaPosition", startTs);
            _lastStartTs = startTs;
        }

        private void doSetWavFilePath()
        {
            MainWindowGuiData.MediaFilePath = new Uri(MainWindowGuiData.WavFilePath);
            App.Messenger.NotifyColleagues("SpeechRecognitionFileChanged", MainWindowGuiData.WavFilePath);
        }

        void doStartSpeechRecognition(string data)
        {
            backupSubtitleFile();
            App.Messenger.NotifyColleagues("StartSpeechRecognition", MainWindowGuiData.WavFilePath);
        }

        void doSync(string data)
        {
            MainWindowGuiData.PopupDoSyncIsOpen = true;
        }

        private void enableButtons()
        {
            MainWindowGuiData.DoMergeIsEnabled = !string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath);
            MainWindowGuiData.DoMixIsEnabled = !string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath);
        }

        bool isFileOpen(string data)
        {
            return !string.IsNullOrEmpty(MainWindowGuiData.OpenedFilePath);
        }

        private void mainWidowIsLoaded()
        {
            if (subtitleItemsDataInternal == null || subtitleItemsDataInternal.Count == 0) return;
            App.Messenger.NotifyColleagues("SubtitleItems", subtitleItemsDataInternal);
        }

        void mainWindowGuiDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SearchText":
                    doSearch(MainWindowGuiData.SearchText.ApplyUnifiedYeKe());
                    break;
                case "OpenedFilePath":
                    openSubtitleFile();
                    break;
                case "MergeFilePath":
                    new Thread(doMerge).Start();
                    break;
                case "MixFilePath":
                    new Thread(doMix).Start();
                    break;
                case "MediaFilePath":
                    setMediaFilePathOpenSubTitle();
                    break;
                case "IsLoaded":
                    mainWidowIsLoaded();
                    break;
                case "SelectedItem":
                    doSelectedItem();
                    break;
                case "WavFilePath":
                    doSetWavFilePath();
                    break;
            }
        }

        private void openSubtitleFile()
        {
            if (string.IsNullOrWhiteSpace(MainWindowGuiData.OpenedFilePath))
            {
                clearSrtPanel();
                MainWindowGuiData.HeaderText = string.Empty;
                return;
            }
            MainWindowGuiData.HeaderText = MainWindowGuiData.OpenedFilePath;
            if (new FileInfo(MainWindowGuiData.OpenedFilePath).Length == 0)
            {
                clearSrtPanel();
                return;
            }

            new Thread(doOpenFile).Start();
        }

        private void processCommandLineArguments()
        {
            var startupFileName = App.StartupFileName;
            if (string.IsNullOrEmpty(startupFileName)) return;
            MainWindowGuiData.OpenedFilePath = startupFileName;
        }

        private void reBind()
        {
            new Thread(doOpenFile).Start();
        }

        private void saveToFile(string path = "")
        {
            var savePath = string.IsNullOrEmpty(path) ? MainWindowGuiData.OpenedFilePath : path;
            var newContent = ParseSrt.SubitemsToString(subtitleItemsDataInternal);
            File.WriteAllText(savePath, newContent.ApplyUnifiedYeKe(), Encoding.UTF8);
            LogWindow.AddMessage(LogType.Announcement, string.Format("Saved to:{0}", savePath));
            setFlowDir(newContent.ContainsFarsi());
        }

        void setFlowDir(bool isRtl)
        {
            MainWindowGuiData.TableFlowDirection = isRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private void setMediaFilePathOpenSubTitle()
        {
            if (MainWindowGuiData.MediaFilePath == null ||
                string.IsNullOrWhiteSpace(MainWindowGuiData.MediaFilePath.LocalPath)) return;

            var subtitleFilePath = Path.ChangeExtension(MainWindowGuiData.MediaFilePath.LocalPath, ".srt");
            if (!File.Exists(subtitleFilePath))
            {
                MainWindowGuiData.OpenedFilePath = string.Empty;
                return;
            }

            MainWindowGuiData.OpenedFilePath = subtitleFilePath;
        }

        private void setupCommands()
        {
            DoConvertToUTF8 = new DelegateCommand<string>(doConvertToUTF8, isFileOpen);
            DoSync = new DelegateCommand<string>(doSync, isFileOpen);
            DoJoinFiles = new DelegateCommand<string>(doJoinFiles, isFileOpen);
            DoDelete = new DelegateCommand<string>(doDelete, isFileOpen);
            DoRecalculate = new DelegateCommand<string>(doRecalculate, isFileOpen);
            DoSaveChanges = new DelegateCommand<string>(doSaveChanges, isFileOpen);
            DoInsertRLE = new DelegateCommand<string>(doInsertRLE, isFileOpen);
            DoStartSpeechRecognition = new DelegateCommand<string>(doStartSpeechRecognition, canDoStartSpeechRecognition);
        }

        private void setupItemsData()
        {
            _changedRows = new SortedSet<int>();
            MainWindowGuiData = new MainWindowGui();
            MainWindowGuiData.PropertyChanged += mainWindowGuiDataPropertyChanged;
            subtitleItemsDataInternal = new SubtitleItems();
            subtitleItemsDataInternal.CollectionChanged += subtitleItemsDataInternalCollectionChanged;
            SubtitleItemsDataView = CollectionViewSource.GetDefaultView(subtitleItemsDataInternal);
            MainWindowGuiData.IsLoaded = true;
        }

        private void setupMessenger()
        {
            App.Messenger.Register("reBind", doRebindMsg);
            App.Messenger.Register("doCloseJoinPopup", doCloseJoinPopup);
            App.Messenger.Register("doCloseSyncView", doCloseSyncView);
            App.Messenger.Register("doCloseConvertEncodingView", doCloseConvertEncodingView);
            App.Messenger.Register<SubtitleItem>("doAddSubtitle", doAddSubtitle);
            App.Messenger.Register<int>("doScrollToIndex", doScrollToIndex);
            App.Messenger.Register<SubtitleItem>("doAddVoiceSubtitle", doAddVoiceSubtitle);
            App.Messenger.Register<string>("doChangeWavFilePath", doChangeWavFilePath);

        }

        private void showConflicts()
        {
            var conflicts = ParseSrt.FindConflicts(_subtitleItemsDataInternal);
            if (!conflicts.Any()) return;

            foreach (var item in conflicts)
            {
                LogWindow.AddMessage(LogType.Alert, item);
            }
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
    }
}
