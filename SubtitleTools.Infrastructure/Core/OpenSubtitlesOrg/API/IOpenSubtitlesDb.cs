using CookComputing.XmlRpc;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API
{
    public interface IOpenSubtitlesDb : IXmlRpcProxy
    {
        [XmlRpcMethod]
        LoginResult LogIn(string username, string password, string language, string useragent);

        [XmlRpcMethod]
        SubtitlesSearchResult SearchSubtitles(string token, SearchInfo[] parameters);

        [XmlRpcMethod]
        MovieCheckHashResult CheckMovieHash2(string token, string[] moviehash);

        [XmlRpcMethod]
        SubCheckHashResult CheckSubHash(string token, string[] subhash);

        [XmlRpcMethod]
        TryUploadResult TryUploadSubtitles(string token, TryUploadInfo[] parameters);

        [XmlRpcMethod]
        DownloadSubtitlesResult DownloadSubtitles(string token, string[] idSubtitleFile);
    }
}
