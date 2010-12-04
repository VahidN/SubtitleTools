using CookComputing.XmlRpc;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class LoginResult
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double seconds { set; get; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string token { set; get; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string status { set; get; }
    }
}