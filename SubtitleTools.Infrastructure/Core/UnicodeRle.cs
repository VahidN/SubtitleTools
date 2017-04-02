using DNTPersianUtils.Core;
using DNTPersianUtils.Core.Normalizer;

namespace SubtitleTools.Infrastructure.Core
{
    public static class UnicodeRle
    {
        public static string InsertRle(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (!text.ContainsFarsi())
                return text;

            return text.ApplyRle()
                       .ApplyCorrectYeKe()
                       .ApplyHalfSpaceRule()
                       .NormalizeZwnj()
                       .NormalizeDashes()
                       .NormalizeDotsToEllipsis()
                       .NormalizeEnglishQuotes()
                       .NormalizeExtraMarks()
                       .NormalizeAllKashida()
                       .NormalizeSpacingAndLineBreaks()
                       .NormalizeOutsideInsideSpacing();
        }
    }
}