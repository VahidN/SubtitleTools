using CookComputing.XmlRpc;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class TryUploadResult
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string status { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int alreadyindb { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public XmlRpcStruct[] data { get; set; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double seconds { get; set; }
    }
}
