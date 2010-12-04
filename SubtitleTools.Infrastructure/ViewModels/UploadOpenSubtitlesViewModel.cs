using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Win32;
using SubtitleTools.Common.Files;
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

        #region Properties (5)

        public DelegateCommand<string> DoSelectMovieFile { set; get; }

        public DelegateCommand<string> DoSelectSubtitleFile { set; get; }

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

        #region Methods (12)

        // Private Methods (12) 

        static bool canDoSelectMovieFile(string data)
        {
            return true;
        }

        static bool canDoSelectSubtitleFile(string data)
        {
            return true;
        }

        bool canDoUploadOneFile(string data)
        {
            return
                !string.IsNullOrEmpty(UploadItemData.MoviePath)
                && !string.IsNullOrEmpty(UploadItemData.SubtitlePath)
                && (UploadItemData.SelectedSubtitleLanguage != null);
        }

        void doSelectMovieFile(string data)
        {
            var dlg = new OpenFileDialog
            {
                Filter = Filters.MovieFilter
            };

            var result = dlg.ShowDialog();
            if (result != true) return;

            UploadItemData.MoviePath = dlg.FileName;

            var subFile = tryToFindSubFile(UploadItemData.MoviePath);
            if (!string.IsNullOrEmpty(subFile))
                UploadItemData.SubtitlePath = subFile;

            tryEnableButtons();
        }

        void doSelectSubtitleFile(string data)
        {
            var dlg = new OpenFileDialog
            {
                Filter = Filters.SrtFilter
            };

            var result = dlg.ShowDialog();
            if (result != true) return;
            UploadItemData.SubtitlePath = dlg.FileName;

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
            DoSelectMovieFile = new DelegateCommand<string>(doSelectMovieFile, canDoSelectMovieFile);
            DoSelectSubtitleFile = new DelegateCommand<string>(doSelectSubtitleFile, canDoSelectSubtitleFile);
            DoUploadOneFile = new DelegateCommand<string>(doUploadOneFile, canDoUploadOneFile);
        }

        void tryEnableButtons()
        {
            DoSelectMovieFile.CanExecute(string.Empty);
            DoSelectSubtitleFile.CanExecute(string.Empty);
            DoUploadOneFile.CanExecute(string.Empty);
        }

        static string tryToFindSubFile(string movieFile)
        {
            var subFile = Path.ChangeExtension(movieFile, ".srt");
            return File.Exists(subFile) ? subFile : string.Empty;
        }

        void uploadItemDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedSubtitleLanguage")
            {
                tryEnableButtons();
            }
        }

        #endregion Methods
    }
}
