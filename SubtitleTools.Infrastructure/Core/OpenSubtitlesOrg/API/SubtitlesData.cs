
using CookComputing.XmlRpc;
namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class SubtitlesData
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string idsubtitlefile { set; get; }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string data { set; get; }
    }
}
