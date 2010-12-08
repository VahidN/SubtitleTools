using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using SubtitleTools.Common.ISO639;
using SubtitleTools.Common.Logger;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class UploadOpenSubtitlesViewModel
    {
        #region Constructors (1)

        public UploadOpenSubtitlesViewModel()
        {
            UploadItemData = new UploadItem();
            UploadItemData.PropertyChanged += uploadItemDataPropertyChanged;
            setupCommands();
        }

        #endregion Constructors

        #region Properties (3)

        public DelegateCommand<string> DoUploadOneFile { set; get; }

        public IList<Language> SubLanguages
        {
            get
            {
                var lc = new LanguagesCodes();
                var item = lc.Where(l => l.LanguageName == "*All Languages*").FirstOrDefault();
                lc.Remove(item);
                return lc.OrderBy(o => o.LanguageName).ToList();
            }
        }

        public UploadItem UploadItemData { set; get; }

        #endregion Properties

        #region Methods (10)

        // Private Methods (10) 

        bool canDoUploadOneFile(string data)
        {
            return
                !string.IsNullOrEmpty(UploadItemData.MoviePath)
                && !string.IsNullOrEmpty(UploadItemData.SubtitlePath)
                && (UploadItemData.SelectedSubtitleLanguage != null);
        }

        void doSelectMovieFile()
        {
            if (string.IsNullOrWhiteSpace(UploadItemData.MoviePath)) return;

            var subFile = tryToFindSubFile(UploadItemData.MoviePath);
            if (!string.IsNullOrEmpty(subFile))
                UploadItemData.SubtitlePath = subFile;

            tryEnableButtons();
        }

        void doSelectSubtitleFile()
        {
            tryEnableButtons();
        }

        private void doUpload()
        {
            try
            {
                UploadItemData.IsBusy = true;
                UploadItemData.FinalUrl = string.Empty;

                var openSubtitlesXmlRpc = new OpenSubtitlesXmlRpc(UploadItemData.MoviePath);
                var finalUrl = openSubtitlesXmlRpc.UploadSubtitle(
                    UploadItemData.SelectedSubtitleLanguage.IdSubLanguage.ToLower(),
                    UploadItemData.SubtitlePath,
                    e => UploadItemData.Progress = e);

                setFinalUrl(finalUrl);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
            finally
            {
                UploadItemData.IsBusy = false;
                UploadItemData.Progress = 0;
            }
        }

        void doUploadOneFile(string data)
        {
            new Thread(doUpload).Start();
        }

        private void setFinalUrl(string finalUrl)
        {
            UploadItemData.FinalUrl = !string.IsNullOrEmpty(finalUrl) ? finalUrl : string.Empty;
        }

        private void setupCommands()
        {
            DoUploadOneFile = new DelegateCommand<string>(doUploadOneFile, canDoUploadOneFile);
        }

        void tryEnableButtons()
        {
            DoUploadOneFile.CanExecute(string.Empty);
        }

        static string tryToFindSubFile(string movieFile)
        {
            var subFile = Path.ChangeExtension(movieFile, ".srt");
            return File.Exists(subFile) ? subFile : string.Empty;
        }

        void uploadItemDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "MoviePath":
                    doSelectMovieFile();
                    break;
                case "SubtitlePath":
                    doSelectSubtitleFile();
                    break;
                case "SelectedSubtitleLanguage":
                    tryEnableButtons();
                    break;
            }
        }

        #endregion Methods
    }
}
