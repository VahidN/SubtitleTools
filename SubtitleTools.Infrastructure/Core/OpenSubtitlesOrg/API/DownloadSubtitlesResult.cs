using CookComputing.XmlRpc;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class DownloadSubtitlesResult
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string status { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public SubtitlesData[] data { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double seconds { get; set; }
    }

}
