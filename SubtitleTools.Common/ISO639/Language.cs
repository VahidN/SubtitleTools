using System.Diagnostics;
namespace SubtitleTools.Common.ISO639
{
    [DebuggerDisplay("{LanguageName}|{ISO639}")]
    public class Language
    {
        public string IdSubLanguage { set; get; }
        public string ISO639 { set; get; }
        public string LanguageName { set; get; }
        public string ISO6393166_1 { set; get; }
    }
}