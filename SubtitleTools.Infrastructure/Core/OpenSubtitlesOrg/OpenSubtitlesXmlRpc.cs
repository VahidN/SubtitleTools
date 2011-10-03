using System;
using System.IO;
using CookComputing.XmlRpc;
using SubtitleTools.Common.Compression;
using SubtitleTools.Common.Config;
using SubtitleTools.Common.Logger;
using SubtitleTools.Common.Net;
using SubtitleTools.Common.Regex;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.Helper;
using SubtitleTools.Common.Toolkit;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg
{
    public class OpenSubtitlesXmlRpc
    {
        #region Fields (3)

        readonly IOpenSubtitlesDb _client;
        string _loginToken;
        readonly string _movieFileName;

        #endregion Fields

        #region Constructors (1)

        public OpenSubtitlesXmlRpc(string movieFileName)
        {
            _movieFileName = movieFileName;
            _client = XmlRpcProxyGen.Create<IOpenSubtitlesDb>();
            _client.Expect100Continue = false;
            _client.UserAgent = "SubtitleTools";
            _client.Url = ConfigSetGet.GetConfigData("OpensubtitlesOrgApiUri");
        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (4) 

        public void DownloadAllSubtitles(Action<int> progress, string subLanguageId = "all")
        {
            var info = GetListOfAllSubtitles(progress, subLanguageId);
            if (info == null || info.data == null)
            {
                return;
            }

            foreach (var item in info.data)
            {
                DownloadSubtitle(item.IDSubtitleFile, item.SubFileName, item.LanguageName, progress);
            }
        }

        public void DownloadSubtitle(string id, string subFileName, string lang, Action<int> progress, bool debugMode = false)
        {
            if (progress != null) progress(10);

            tryLogin();

            if (progress != null) progress(40);

            LogWindow.AddMessage(LogType.Info, string.Format("DownloadSubtitle({0})", id));
            var result = _client.DownloadSubtitles(_loginToken, new[] { id });

            if (progress != null) progress(75);

            if (result.status == "200 OK")
            {
                if (result.data == null || result.data.Length == 0) return;

                var gzBase64Data = result.data[0].data;
                if (string.IsNullOrWhiteSpace(gzBase64Data))
                {
                    throw new Exception("Received gzBase64Data is empty.");
                }

                var fileName = string.Format(@"{0}\{1}-{2}-{3}", Path.GetDirectoryName(_movieFileName), id, lang, subFileName);

                if (debugMode)
                {
                    File.WriteAllText(string.Format("{0}.base64gz", fileName), gzBase64Data);
                }

                //from: http://trac.opensubtitles.org/projects/opensubtitles/wiki/XmlRpcIntro
                //it's gzipped without header.
                var gzBuffer = Convert.FromBase64String(gzBase64Data);
                var content = Compression.DecompressGz(gzBuffer);
                LogWindow.AddMessage(LogType.Announcement, string.Format("Saved to:  {0}", fileName));
                File.WriteAllBytes(fileName, content);
            }
            else
            {
                throw new Exception(string.Format("Status: {0}. It's not possible to download {1}", result.status, subFileName));
            }

            if (progress != null) progress(100);
        }

        public SubtitlesSearchResult GetListOfAllSubtitles(Action<int> progress, string subLanguageId = "all")
        {
            var fileInfo = new MovieFileInfo(_movieFileName, subFileName: string.Empty);
            //get file info            
            var movieHash = fileInfo.MovieHash;
            LogWindow.AddMessage(LogType.Info, string.Format("MovieHash: {0}", movieHash));
            var fileLen = fileInfo.MovieFileLength;

            //login
            if (progress != null) progress(25);

            LogWindow.AddMessage(LogType.Info, "TryLogin ...");
            tryLogin();

            if (progress != null) progress(50);

            //has any subtitle?
            var token = _loginToken;
            var hashInfo = _client.CheckMovieHash2(token, new[] { movieHash });
            var tryConvertToObjArr = hashInfo.data as object[];
            if ((tryConvertToObjArr != null) && (tryConvertToObjArr.Length == 0))
            {
                throw new Exception("This movie has not any subtitle.");
            }

            if (progress != null) progress(75);

            LogWindow.AddMessage(LogType.Info, "SearchSubtitles ... ");

            //get more info
            SubtitlesSearchResult result;

            try
            {
                result = _client.SearchSubtitles(
                    token,
                    new[] 
                { 
                    new SearchInfo
                    {
                        moviehash = movieHash,
                        sublanguageid = subLanguageId,
                        moviebytesize = fileLen
                    }
                }
                );
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogExceptionToFile(ex);
                LogWindow.AddMessage(LogType.Alert, "Found Nothing!");
                if (progress != null) progress(100);
                return null;
            }

            if (result != null && result.data != null && result.data.Length > 0)
            {
                LogWindow.AddMessage(LogType.Announcement, string.Format("Found {0} Subtitle(s).", result.data.Length));
            }
            else
            {
                LogWindow.AddMessage(LogType.Alert, "Found Nothing!");
            }

            if (progress != null) progress(100);

            return result;
        }

        public string UploadSubtitle(long userImdbId, string subLanguageId, string subFileNamePath, Action<int> progress)
        {
            string finalUrl;
            subFileNamePath = new ChangeEncoding().TryReduceRtlLargeFileContent(subFileNamePath);
            var fileInfo = new MovieFileInfo(_movieFileName, subFileNamePath);
            //login            
            if (progress != null) progress(10);

            LogWindow.AddMessage(LogType.Info, "TryLogin ...");
            tryLogin();

            if (progress != null) progress(25);

            LogWindow.AddMessage(LogType.Info, "TryUploadSubtitle ...");

            TryUploadResult res = null;
            try
            {
                res = _client.TryUploadSubtitles(_loginToken,
                    new[]
                        { 
                            new TryUploadInfo
                            {
                               subhash = fileInfo.SubtitleHash,
                               subfilename = fileInfo.SubFileName,
                               moviehash = fileInfo.MovieHash,
                               moviebytesize = fileInfo.MovieFileLength,
                               moviefilename = fileInfo.MovieFileName
                            }
                        }
                    );
            }
            catch (Exception ex)
            {
                if (!processNewMovieFile(ref userImdbId, fileInfo, ref res, ex))
                    throw;
            }

            if (res == null)
            {
                throw new InvalidOperationException("Bad response ...");
            }

            if (progress != null) progress(50);

            if (res.status != "200 OK")
            {
                throw new Exception("Bad response ...");
            }

            if (res.alreadyindb == 0)
            {
                if ((userImdbId == 0) && (res.data == null || res.data.Length == 0))
                {
                    throw new Exception("Bad format ...");
                }

                LogWindow.AddMessage(LogType.Info, string.Format("CheckSubHash({0})", fileInfo.SubtitleHash));
                var checkSubHashRes = _client.CheckSubHash(_loginToken, new[] { fileInfo.SubtitleHash });

                if (progress != null) progress(75);

                var idSubtitleFile = int.Parse(checkSubHashRes.data[fileInfo.SubtitleHash].ToString());
                if (idSubtitleFile > 0)
                {
                    throw new Exception("Duplicate subHash, alreadyindb.");
                }

                LogWindow.AddMessage(LogType.Info, "PostData ...");
                //xml-rpc.net dll does not work here so, ...
                var post = PostXml.PostData(
                     ConfigSetGet.GetConfigData("OpensubtitlesOrgApiUri"),
                     UploadData.CreateUploadXml(_loginToken,
                     new UploadBaseinfo
                     {
                         idmovieimdb = res.data != null ? res.data[0]["IDMovieImdb"].ToString() : userImdbId.ToString(),
                         sublanguageid = subLanguageId,
                         movieaka = string.Empty,
                         moviereleasename = fileInfo.MovieReleaseName,
                         subauthorcomment = string.Empty
                     },
                     new UploadCDsInfo
                     {
                         moviebytesize = fileInfo.MovieFileLength.ToString(),
                         moviefilename = fileInfo.MovieFileName,
                         moviehash = fileInfo.MovieHash,
                         subfilename = fileInfo.SubFileName,
                         subhash = fileInfo.SubtitleHash,
                         subcontent = fileInfo.SubContentToUpload,
                         moviefps = string.Empty,
                         movieframes = string.Empty,
                         movietimems = string.Empty
                     }));

                LogWindow.AddMessage(LogType.Info, "Done!");
                finalUrl = RegexHelper.GetUploadUrl(post);
                LogWindow.AddMessage(LogType.Announcement, string.Format("Url: {0}", finalUrl));
            }
            else
            {
                throw new Exception("Duplicate file, alreadyindb");
            }

            if (progress != null) progress(100);
            return finalUrl.Trim();
        }

        // Private Methods (2) 

        private static bool processNewMovieFile(ref long userImdbId, MovieFileInfo fileInfo, ref TryUploadResult res, Exception ex)
        {
            if (ex.Message.Contains("response contains boolean value where array expected")) // what did you expect from PHP developers?!
            {
                if (userImdbId == 0)
                {
                    userImdbId = Imdb.GetImdbId(Path.GetFileNameWithoutExtension(fileInfo.MovieFileName));
                    if (userImdbId == 0)
                    {
                        throw new NotSupportedException("It's a new movie file. Please find/fill its IMDB Id first.");
                    }
                    LogWindow.AddMessage(LogType.Info, string.Format("ImdbId: {0}", userImdbId));
                }

                //it's a new movie file and site's db has no info (IDMovieImdb val) about it.
                res = new TryUploadResult { data = null, status = "200 OK", alreadyindb = 0 };
                return true;
            }
            return false;
        }

        void tryLogin()
        {
            if (!string.IsNullOrEmpty(_loginToken)) return;

            var loginInfo = _client.LogIn(string.Empty, string.Empty, string.Empty, UA.MyUA);
            var status = loginInfo.status;
            if (string.IsNullOrWhiteSpace(status) || status != "200 OK")
            {
                throw new Exception("Couldn't login.");
            }
            _loginToken = loginInfo.token;
        }

        #endregion Methods
    }
}
