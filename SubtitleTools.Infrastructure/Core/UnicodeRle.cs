using PersianProofWriter.Lib;

namespace SubtitleTools.Infrastructure.Core
{
    public static class UnicodeRle
    {
        public static string InsertRle(this string text)
        {
            return text.ApplyAllPersianRulesPlusRle();
        }
    }
}
