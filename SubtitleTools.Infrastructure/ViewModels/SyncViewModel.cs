using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class SyncViewModel
    {
        #region Constructors (1)

        public SyncViewModel()
        {
            SyncModelData = new SyncModel();
            setupCommands();
        }

        #endregion Constructors

        #region Properties (3)

        public DelegateCommand<string> DoCloseSyncView { set; get; }

        public DelegateCommand<string> DoSync { set; get; }

        public SyncModel SyncModelData { set; get; }

        #endregion Properties

        #region Methods (5)

        // Private Methods (5) 

        bool canDoCloseSyncView(string data)
        {
            return true;
        }

        static bool canDoSync(string filePatha)
        {
            return true;
        }

        void doCloseSyncView(string data)
        {
            App.Messenger.NotifyColleagues("doCloseSyncView");
        }

        void doSync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            if (SyncModelData.AddIndex == -1)
            {
                LogWindow.AddMessage(LogType.Alert, "Please select the appropriate shift direction.");
                return;
            }

            Sync.ShiftFileTimeLines(
                filePath,
                SyncModelData.Hour,
                SyncModelData.Minutes,
                SyncModelData.Seconds,
                SyncModelData.Milliseconds,
                SyncModelData.AddIndex == 0 ? true : false);

            App.Messenger.NotifyColleagues("reBind");
        }

        private void setupCommands()
        {
            DoSync = new DelegateCommand<string>(doSync, canDoSync);
            DoCloseSyncView = new DelegateCommand<string>(doCloseSyncView, canDoCloseSyncView);
        }

        #endregion Methods
    }
}
