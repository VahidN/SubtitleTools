using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using SubtitleTools.Common.CodePlexRss;
using SubtitleTools.Common.CodePlexRss.Model;
using SubtitleTools.Common.Config;
using SubtitleTools.Common.Logger;
using SubtitleTools.Common.Net;
using SubtitleTools.Infrastructure.Core;


namespace SubtitleTools.Infrastructure.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        #region Fields (2)

        bool _isBusy;
        VersionsInfo _versionsInfoData;

        #endregion Fields

        #region Constructors (1)

        public AboutViewModel()
        {
            VersionsInfoData = new VersionsInfo();
            new Thread(downloadInfo).Start();
        }

        #endregion Constructors

        #region Properties (2)

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

        public VersionsInfo VersionsInfoData
        {
            set
            {
                _versionsInfoData = value;
                raisePropertyChanged("VersionsInfoData");
            }
            get { return _versionsInfoData; }
        }

        #endregion Properties

        #region Methods (3)

        // Private Methods (3) 

        private static string cacheManager(string rssXml)
        {
            string localCacheFile = SubtitleTools.Common.Files.Path.AppPath + "\\rss.xml";

            if (string.IsNullOrWhiteSpace(rssXml))
            {
                //try using a local cache
                if (File.Exists(localCacheFile))
                    rssXml = File.ReadAllText(localCacheFile);
            }
            else
            {
                //cache it
                File.WriteAllText(localCacheFile, rssXml);
            }

            return rssXml;
        }

        void downloadInfo()
        {
            try
            {
                IsBusy = true;

                string rssXml = tryDownloadRss();
                rssXml = cacheManager(rssXml);

                if (string.IsNullOrWhiteSpace(rssXml))
                {
                    IsBusy = false;
                    return;
                }

                VersionsInfoData.Clear();
                VersionsInfoData = DownloadHistory.ParseInfo(rssXml);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Error, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private static string tryDownloadRss()
        {
            var rssXml = string.Empty;
            try
            {
                var url = ConfigSetGet.GetConfigData("ProjectRSSFeed");
                rssXml = Downloader.FetchWebPage(url);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("The remote name could not be resolved"))
                {
                    ExceptionLogger.LogExceptionToFile(ex);
                    LogWindow.AddMessage(LogType.Error, ex.Message);
                }
            }
            return rssXml;
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
