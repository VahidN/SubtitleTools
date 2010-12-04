using CookComputing.XmlRpc;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class UploadResult
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double seconds { set; get; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string data { set; get; }
                
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string status { set; get; }
    }
}
