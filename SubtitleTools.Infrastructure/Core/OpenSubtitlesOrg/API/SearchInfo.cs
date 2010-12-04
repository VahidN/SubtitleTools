using CookComputing.XmlRpc;
namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class SearchInfo
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string sublanguageid { set; get; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string moviehash { set; get; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double moviebytesize { set; get; }
    }
}