using System.Collections.Generic;

namespace SubtitleTools.Common.ISO639
{
    //from: http://www.opensubtitles.org/addons/export_languages.php
    public class LanguagesCodes : List<Language>
    {
        public LanguagesCodes()
        {            
            this.Add(new Language { IdSubLanguage = "all", ISO639 = "", LanguageName = "*All Languages*", ISO6393166_1 = "null" });
            this.Add(new Language { IdSubLanguage = "alb", ISO639 = "sq", LanguageName = "Albanian", ISO6393166_1 = "AL" });
            this.Add(new Language { IdSubLanguage = "ara", ISO639 = "ar", LanguageName = "Arabic", ISO6393166_1 = "SA" });
            this.Add(new Language { IdSubLanguage = "arm", ISO639 = "hy", LanguageName = "Armenian", ISO6393166_1 = "AM" });
            this.Add(new Language { IdSubLanguage = "ben", ISO639 = "bn", LanguageName = "Bengali", ISO6393166_1 = "BD" });
            this.Add(new Language { IdSubLanguage = "bos", ISO639 = "bs", LanguageName = "Bosnian", ISO6393166_1 = "BA" });
            this.Add(new Language { IdSubLanguage = "bul", ISO639 = "bg", LanguageName = "Bulgarian", ISO6393166_1 = "BG" });
            this.Add(new Language { IdSubLanguage = "cat", ISO639 = "ca", LanguageName = "Catalan", ISO6393166_1 = "AD" });
            this.Add(new Language { IdSubLanguage = "chi", ISO639 = "zh", LanguageName = "Chinese", ISO6393166_1 = "CN" });
            this.Add(new Language { IdSubLanguage = "cze", ISO639 = "cs", LanguageName = "Czech", ISO6393166_1 = "CZ" });
            this.Add(new Language { IdSubLanguage = "dan", ISO639 = "da", LanguageName = "Danish", ISO6393166_1 = "DK" });
            this.Add(new Language { IdSubLanguage = "dut", ISO639 = "nl", LanguageName = "Dutch", ISO6393166_1 = "DE" });
            this.Add(new Language { IdSubLanguage = "eng", ISO639 = "en", LanguageName = "English", ISO6393166_1 = "GB" });
            this.Add(new Language { IdSubLanguage = "epo", ISO639 = "eo", LanguageName = "Esperanto", ISO6393166_1 = "" });
            this.Add(new Language { IdSubLanguage = "est", ISO639 = "et", LanguageName = "Estonian", ISO6393166_1 = "EE" });
            this.Add(new Language { IdSubLanguage = "fin", ISO639 = "fi", LanguageName = "Finnish", ISO6393166_1 = "FI" });
            this.Add(new Language { IdSubLanguage = "fre", ISO639 = "fr", LanguageName = "French", ISO6393166_1 = "FR" });
            this.Add(new Language { IdSubLanguage = "geo", ISO639 = "ka", LanguageName = "Georgian", ISO6393166_1 = "GB" });
            this.Add(new Language { IdSubLanguage = "ger", ISO639 = "de", LanguageName = "German", ISO6393166_1 = "DE" });
            this.Add(new Language { IdSubLanguage = "glg", ISO639 = "gl", LanguageName = "Galician", ISO6393166_1 = "ES" });
            this.Add(new Language { IdSubLanguage = "ell", ISO639 = "el", LanguageName = "Greek", ISO6393166_1 = "GR" });
            this.Add(new Language { IdSubLanguage = "heb", ISO639 = "he", LanguageName = "Hebrew", ISO6393166_1 = "IL" });
            this.Add(new Language { IdSubLanguage = "hin", ISO639 = "hi", LanguageName = "Hindi", ISO6393166_1 = "IN" });
            this.Add(new Language { IdSubLanguage = "hrv", ISO639 = "hr", LanguageName = "Croatian", ISO6393166_1 = "HR" });
            this.Add(new Language { IdSubLanguage = "hun", ISO639 = "hu", LanguageName = "Hungarian", ISO6393166_1 = "HU" });
            this.Add(new Language { IdSubLanguage = "ice", ISO639 = "is", LanguageName = "Icelandic", ISO6393166_1 = "IS" });
            this.Add(new Language { IdSubLanguage = "ind", ISO639 = "id", LanguageName = "Indonesian", ISO6393166_1 = "ID" });
            this.Add(new Language { IdSubLanguage = "ita", ISO639 = "it", LanguageName = "Italian", ISO6393166_1 = "IT" });
            this.Add(new Language { IdSubLanguage = "jpn", ISO639 = "ja", LanguageName = "Japanese", ISO6393166_1 = "JP" });
            this.Add(new Language { IdSubLanguage = "kaz", ISO639 = "kk", LanguageName = "Kazakh", ISO6393166_1 = "KZ" });
            this.Add(new Language { IdSubLanguage = "kor", ISO639 = "ko", LanguageName = "Korean", ISO6393166_1 = "KR" });
            this.Add(new Language { IdSubLanguage = "lav", ISO639 = "lv", LanguageName = "Latvian", ISO6393166_1 = "LV" });
            this.Add(new Language { IdSubLanguage = "lit", ISO639 = "lt", LanguageName = "Lithuanian", ISO6393166_1 = "LT" });
            this.Add(new Language { IdSubLanguage = "ltz", ISO639 = "lb", LanguageName = "Luxembourgish", ISO6393166_1 = "LU" });
            this.Add(new Language { IdSubLanguage = "mac", ISO639 = "mk", LanguageName = "Macedonian", ISO6393166_1 = "MK" });
            this.Add(new Language { IdSubLanguage = "may", ISO639 = "ms", LanguageName = "Malay", ISO6393166_1 = "MY" });
            this.Add(new Language { IdSubLanguage = "nor", ISO639 = "no", LanguageName = "Norwegian", ISO6393166_1 = "NO" });
            this.Add(new Language { IdSubLanguage = "oci", ISO639 = "oc", LanguageName = "Occitan", ISO6393166_1 = "IT" });
            this.Add(new Language { IdSubLanguage = "per", ISO639 = "fa", LanguageName = "Persian", ISO6393166_1 = "IR" });
            this.Add(new Language { IdSubLanguage = "pol", ISO639 = "pl", LanguageName = "Polish", ISO6393166_1 = "PL" });
            this.Add(new Language { IdSubLanguage = "por", ISO639 = "pt", LanguageName = "Portuguese", ISO6393166_1 = "PT" });
            this.Add(new Language { IdSubLanguage = "rus", ISO639 = "ru", LanguageName = "Russian", ISO6393166_1 = "RU" });
            this.Add(new Language { IdSubLanguage = "scc", ISO639 = "sr", LanguageName = "Serbian", ISO6393166_1 = "RS" });
            this.Add(new Language { IdSubLanguage = "sin", ISO639 = "si", LanguageName = "Sinhalese", ISO6393166_1 = "LK" });
            this.Add(new Language { IdSubLanguage = "slo", ISO639 = "sk", LanguageName = "Slovak", ISO6393166_1 = "SK" });
            this.Add(new Language { IdSubLanguage = "slv", ISO639 = "sl", LanguageName = "Slovenian", ISO6393166_1 = "SI" });
            this.Add(new Language { IdSubLanguage = "spa", ISO639 = "es", LanguageName = "Spanish", ISO6393166_1 = "ES" });
            this.Add(new Language { IdSubLanguage = "swe", ISO639 = "sv", LanguageName = "Swedish", ISO6393166_1 = "SE" });
            this.Add(new Language { IdSubLanguage = "syr", ISO639 = "sy", LanguageName = "Syriac", ISO6393166_1 = "SY" });
            this.Add(new Language { IdSubLanguage = "tgl", ISO639 = "tl", LanguageName = "Tagalog", ISO6393166_1 = "PH" });
            this.Add(new Language { IdSubLanguage = "tha", ISO639 = "th", LanguageName = "Thai", ISO6393166_1 = "TH" });
            this.Add(new Language { IdSubLanguage = "tur", ISO639 = "tr", LanguageName = "Turkish", ISO6393166_1 = "TR" });
            this.Add(new Language { IdSubLanguage = "ukr", ISO639 = "uk", LanguageName = "Ukrainian", ISO6393166_1 = "UA" });
            this.Add(new Language { IdSubLanguage = "urd", ISO639 = "ur", LanguageName = "Urdu", ISO6393166_1 = "AF" });
            this.Add(new Language { IdSubLanguage = "vie", ISO639 = "vi", LanguageName = "Vietnamese", ISO6393166_1 = "VN" });
            this.Add(new Language { IdSubLanguage = "rum", ISO639 = "ro", LanguageName = "Romanian", ISO6393166_1 = "RO" });
            this.Add(new Language { IdSubLanguage = "pob", ISO639 = "pb", LanguageName = "Brazilian", ISO6393166_1 = "BR" });
        }
    }
}
