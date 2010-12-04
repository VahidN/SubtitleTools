using CookComputing.XmlRpc;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class MovieCheckHashResult
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double seconds { set; get; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public object data { set; get; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string status { set; get; }
    }
}