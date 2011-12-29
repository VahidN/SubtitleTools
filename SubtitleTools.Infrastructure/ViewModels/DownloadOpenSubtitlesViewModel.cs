using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SubtitleTools.Common.Files;
using SubtitleTools.Common.ISO639;
using SubtitleTools.Common.Logger;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg;
using SubtitleTools.Infrastructure.Models;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API;

namespace SubtitleTools.Infrastructure.ViewModels
{
    using System.Globalization;

    public class DownloadOpenSubtitlesViewModel : ViewModelBase
    {
        #region Fields (2)

        bool _downloadSelectedFilesIsBusy;
        OsdbItems _osdbItemsData;

        #endregion Fields

        #region Constructors (1)

        public DownloadOpenSubtitlesViewModel()
        {
            DownloadOpenSubtitlesGuiData = new DownloadOpenSubtitlesGui();
            OsdbItemsData = new OsdbItems();
            setupCommands();
        }

        #endregion Constructors

        #region Properties (7)

        public DelegateCommand<string> DoDebugDownloadSelectedItem { set; get; }

        public DelegateCommand<string> DoDownloadSelectedItem { set; get; }

        public DelegateCommand<string> DoDownloadSubtitle { set; get; }

        public DelegateCommand<string> DoSearch { set; get; }

        public DownloadOpenSubtitlesGui DownloadOpenSubtitlesGuiData { set; get; }

        public OsdbItems OsdbItemsData
        {
            set
            {
                _osdbItemsData = value;
                RaisePropertyChanged("OsdbItemsData");
            }
            get { return _osdbItemsData; }
        }

        public IList<Language> SubLanguages
        {
            get
            {
                var lc = new LanguagesCodes();
                return lc.OrderBy(o => o.LanguageName).ToList();
            }
        }

        #endregion Properties

        #region Methods (15)

        // Private Methods (15) 

        static bool canDoDebugDownloadSelectedItem(string data)
        {
            return true;
        }

        static bool canDoDownloadSelectedItem(string data)
        {
            return true;
        }

        bool canDoDownloadSubtitle(string path)
        {
            return !string.IsNullOrWhiteSpace(DownloadOpenSubtitlesGuiData.MoviePath);
        }

        bool canDoSearch(string data)
        {
            return !string.IsNullOrWhiteSpace(DownloadOpenSubtitlesGuiData.MoviePath);
        }

        void doDebugDownloadSelectedItem(string data)
        {
            if (DownloadOpenSubtitlesGuiData.SelectedOsdbItem == null || DownloadOpenSubtitlesGuiData.SelectedOsdbItem.IDSubtitleFile == 0)
                return;

            new Thread(downloadSelectedItem).Start(true);
        }

        void doDownloadSelectedItem(string data)
        {
            if (DownloadOpenSubtitlesGuiData.SelectedOsdbItem == null || DownloadOpenSubtitlesGuiData.SelectedOsdbItem.IDSubtitleFile == 0)
                return;

            new Thread(downloadSelectedItem).Start(false);
        }

        void doDownloadSubtitle(string path)
        {
            new Thread(downloadSelectedFiles).Start();
        }

        void doSearch(string data)
        {
            new Thread(searchGetSubsInfo).Start();
        }

        private void downloadSelectedFiles()
        {
            if (_downloadSelectedFilesIsBusy)
                return;

            try//it's mandatory for threading
            {
                if (string.IsNullOrWhiteSpace(DownloadOpenSubtitlesGuiData.MoviePath))
                    return;

                _downloadSelectedFilesIsBusy = true;

                var osdb = new OpenSubtitlesXmlRpc(DownloadOpenSubtitlesGuiData.MoviePath);

                foreach (var sub in OsdbItemsData)
                {
                    if (!sub.IsSelected) continue;
                    var localSub = sub;
                    osdb.DownloadSubtitle(
                        sub.IDSubtitleFile.ToString(CultureInfo.InvariantCulture),
                        sub.SubFileName,
                        sub.LanguageName,
                        e => localSub.Progress = e
                        );
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
            finally
            {
                _downloadSelectedFilesIsBusy = false;
            }
        }

        private void downloadSelectedItem(object debugMode)
        {
            try//it's mandatory for threading
            {
                if (string.IsNullOrWhiteSpace(DownloadOpenSubtitlesGuiData.MoviePath))
                    return;

                var osdb = new OpenSubtitlesXmlRpc(DownloadOpenSubtitlesGuiData.MoviePath);
                osdb.DownloadSubtitle(
                    DownloadOpenSubtitlesGuiData.SelectedOsdbItem.IDSubtitleFile.ToString(CultureInfo.InvariantCulture),
                    DownloadOpenSubtitlesGuiData.SelectedOsdbItem.SubFileName,
                    DownloadOpenSubtitlesGuiData.SelectedOsdbItem.LanguageName,
                    e => DownloadOpenSubtitlesGuiData.SelectedOsdbItem.Progress = e,
                    (bool)debugMode
                    );
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
        }

        private string getIsoCode(SubtitleDataInfo item)
        {
            var lang = SubLanguages.FirstOrDefault(l => l.LanguageName == item.LanguageName);
            var iso6393166_1 = string.Empty;
            if (lang != null)
            {
                iso6393166_1 = lang.ISO6393166_1;
            }
            return iso6393166_1;
        }

        private void searchGetSubsInfo()
        {
            try //it's mandatory for threading
            {
                if (string.IsNullOrWhiteSpace(DownloadOpenSubtitlesGuiData.MoviePath))
                    return;

                DownloadOpenSubtitlesGuiData.IsBusy = true;
                DownloadOpenSubtitlesGuiData.Progress = 0;

                OsdbItemsData.Clear();
                RaisePropertyChanged("OsdbItemsData");

                var osdb = new OpenSubtitlesXmlRpc(DownloadOpenSubtitlesGuiData.MoviePath);

                var subLanguageId = selectLang();

                var result = osdb.GetListOfAllSubtitles(e => DownloadOpenSubtitlesGuiData.Progress = e, subLanguageId);

                if (result == null || result.data == null || result.data.Length == 0)
                {
                    DownloadOpenSubtitlesGuiData.IsBusy = false;
                    return;
                }

                //sort by LanguageName then by SubAddDate
                var subtitles = result.data.OrderBy(o => o.LanguageName).ThenBy(o => o.SubAddDate).Distinct().ToArray();

                setMovieInfo(result);

                OsdbItemsData.Clear();
                foreach (var item in subtitles)
                {
                    var iso6393166_1 = getIsoCode(item);

                    OsdbItemsData.Add(new OsdbItem
                    {
                        IDSubtitleFile = int.Parse(item.IDSubtitleFile),
                        LanguageName = item.LanguageName,
                        SubAddDate = item.SubAddDate,
                        ISO639 = item.ISO639,
                        ISO6393166_1 = iso6393166_1,
                        SubFileName = item.SubFileName,
                        SubSize = Info.FormatSize(Convert.ToDouble(item.SubSize))
                    });
                }

                RaisePropertyChanged("OsdbItemsData");
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
            finally
            {
                DownloadOpenSubtitlesGuiData.IsBusy = false;
                DownloadOpenSubtitlesGuiData.Progress = 0;
            }
        }

        private string selectLang()
        {
            var subLanguageId = "all";
            if (DownloadOpenSubtitlesGuiData.SubLanguage != null)
            {
                subLanguageId = DownloadOpenSubtitlesGuiData.SubLanguage.LanguageName == "*All Languages*"
                    ? "all" : DownloadOpenSubtitlesGuiData.SubLanguage.IdSubLanguage.ToLower();
            }
            return subLanguageId;
        }

        private void setMovieInfo(SubtitlesSearchResult result)
        {
            if (result != null && result.data != null && result.data.Length > 0)
            {
                DownloadOpenSubtitlesGuiData.MovieYear = result.data[0].MovieYear;
                DownloadOpenSubtitlesGuiData.MovieName = result.data[0].MovieName;
                DownloadOpenSubtitlesGuiData.ImdbRating = result.data[0].MovieImdbRating;
                DownloadOpenSubtitlesGuiData.OsdbUrl = string.Format("http://www.opensubtitles.org/search/sublanguageid-all/moviehash-{0}", result.data[0].MovieHash);
                DownloadOpenSubtitlesGuiData.ImdbUrl = string.Format("http://www.imdb.com/title/tt{0}/", Convert.ToInt32(result.data[0].IDMovieImdb).ToString("0000000"));
            }
        }

        private void setupCommands()
        {
            DoDownloadSubtitle = new DelegateCommand<string>(doDownloadSubtitle, canDoDownloadSubtitle);
            DoSearch = new DelegateCommand<string>(doSearch, canDoSearch);
            DoDownloadSelectedItem = new DelegateCommand<string>(doDownloadSelectedItem, canDoDownloadSelectedItem);
            DoDebugDownloadSelectedItem = new DelegateCommand<string>(doDebugDownloadSelectedItem, canDoDebugDownloadSelectedItem);
        }

        #endregion Methods
    }
}
