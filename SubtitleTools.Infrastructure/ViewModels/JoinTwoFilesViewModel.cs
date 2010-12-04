using System;
using Microsoft.Win32;
using SubtitleTools.Common.Files;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class JoinTwoFilesViewModel
    {
        #region Constructors (1)

        public JoinTwoFilesViewModel()
        {
            JoinTwoFilesGuiData = new JoinTwoFilesGui();
            setupCommands();
        }

        #endregion Constructors

        #region Properties (4)

        public DelegateCommand<string> DoCloseJoinPopup { set; get; }

        public DelegateCommand<string> DoJoin { set; get; }

        public DelegateCommand<string> DoSelectFile { set; get; }

        public JoinTwoFilesGui JoinTwoFilesGuiData { set; get; }

        #endregion Properties

        #region Methods (9)

        // Private Methods (9) 

        bool canDoCloseJoinPopup(string data)
        {
            return true;
        }

        bool canDoJoin(string firstFilePath)
        {
            return !string.IsNullOrWhiteSpace(JoinTwoFilesGuiData.SubtitlePath);
        }

        static bool canDoSelectFile(string data)
        {
            return true;
        }

        private void detectStartTime(string firstFilePath)
        {
            var result = JoinFiles.DetectStartTimeOfSecondFile(firstFilePath, JoinTwoFilesGuiData.SubtitlePath);
            var startTs = result.Item3;
            JoinTwoFilesGuiData.Hour = startTs.Hours;
            JoinTwoFilesGuiData.Minutes = startTs.Minutes;
            JoinTwoFilesGuiData.Seconds = startTs.Seconds;
            JoinTwoFilesGuiData.Milliseconds = startTs.Milliseconds;
        }

        void doCloseJoinPopup(string data)
        {
            App.Messenger.NotifyColleagues("doCloseJoinPopup");
        }

        void doJoin(string firstFilePath)
        {
            JoinFiles.JoinTwoFiles(firstFilePath, JoinTwoFilesGuiData.SubtitlePath,
                new TimeSpan(
                    0,
                    JoinTwoFilesGuiData.Hour,
                    JoinTwoFilesGuiData.Minutes,
                    JoinTwoFilesGuiData.Seconds,
                    JoinTwoFilesGuiData.Milliseconds
                    )
              );
        }

        void doSelectFile(string firstFilePath)
        {
            if (string.IsNullOrWhiteSpace(firstFilePath))
                return;

            var dlg = new OpenFileDialog
            {
                Filter = Filters.SrtFilter
            };

            var result = dlg.ShowDialog();
            if (result != true) return;

            JoinTwoFilesGuiData.SubtitlePath = dlg.FileName;
            detectStartTime(firstFilePath);
            enableButton();
        }

        private void enableButton()
        {
            DoJoin.CanExecute(JoinTwoFilesGuiData.SubtitlePath);
        }

        private void setupCommands()
        {
            DoSelectFile = new DelegateCommand<string>(doSelectFile, canDoSelectFile);
            DoJoin = new DelegateCommand<string>(doJoin, canDoJoin);
            DoCloseJoinPopup = new DelegateCommand<string>(doCloseJoinPopup, canDoCloseJoinPopup);
        }

        #endregion Methods
    }
}
