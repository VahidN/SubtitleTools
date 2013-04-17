using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft;
using SubtitleTools.Common.ISO639;
using SubtitleTools.Common.Toolkit;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.Core
{
    public static class Translate
    {
        private const string AccountKey = "PKWTH4J8g2xmPGTHmQOAlyQsi/aJmef+u5iiuu7rwjo=";
        private static readonly Uri ServiceUri = new Uri("https://api.datamarket.azure.com/Bing/MicrosoftTranslator/");
        private static readonly object _locker = new object();
        private static bool _isRunning;

        public static IList<SubtitleTools.Common.ISO639.Language> GetSupportedLanguages()
        {
            var translatorContainer = new TranslatorContainer(ServiceUri)
            {
                Credentials = new NetworkCredential(AccountKey, AccountKey)
            };
            var languagesForTranslationList = translatorContainer.GetLanguagesForTranslation().Execute().ToList();

            var iso639LanguagesCodes = new LanguagesCodes();
            var supportedLanguages = new List<SubtitleTools.Common.ISO639.Language>();
            foreach (var item in languagesForTranslationList)
            {
                var lang = iso639LanguagesCodes.FirstOrDefault(x => x.ISO639.Equals(item.Code, StringComparison.InvariantCultureIgnoreCase));
                var languageName = lang == null ? item.Code : lang.LanguageName;
                supportedLanguages.Add(new SubtitleTools.Common.ISO639.Language
                {
                    ISO639 = item.Code,
                    LanguageName = languageName,
                    IdSubLanguage = lang == null ? string.Empty : lang.IdSubLanguage,
                    ISO6393166_1 = lang == null ? string.Empty : lang.ISO6393166_1
                });
            }
            return supportedLanguages;
        }

        public static bool StopTranslation { set; get; }

        public static void TranslateAll(string fileName, string sourceLanguage, string targetLanguage)
        {
            lock (_locker)
            {
                if (_isRunning)
                {
                    LogWindow.AddMessage(LogType.Alert, "Translation is in progress...");
                    return;
                }
                _isRunning = true;
                StopTranslation = false;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    LogWindow.AddMessage(LogType.Alert, "Please open a file.");
                    return;
                }

                var data = new ParseSrt().ToObservableCollectionFromFile(fileName);
                if (data == null || !data.Any())
                {
                    LogWindow.AddMessage(LogType.Alert, "Please open a file.");
                    return;
                }

                for (var itemId = 0; itemId < data.Count; itemId++)
                {
                    if (StopTranslation)
                        break;
                    translateItem(sourceLanguage, targetLanguage, data, itemId);
                }

                var saveToFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + targetLanguage + Path.GetExtension(fileName);
                File.WriteAllText(saveToFileName, ParseSrt.SubitemsToString(data).ApplyUnifiedYeKe(), Encoding.UTF8);
                LogWindow.AddMessage(LogType.Info, "Finished Translating. Saved to " + saveToFileName);
            }
            finally
            {
                _isRunning = false;
            }
        }

        private static void translateItem(string sourceLanguage, string targetLanguage, SubtitleItems data, int itemId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data[itemId].Dialog))
                    return;

                LogWindow.AddMessage(LogType.Info, "Translating: " + data[itemId].Dialog);

                var translatorContainer = new TranslatorContainer(ServiceUri)
                {
                    Credentials = new NetworkCredential(AccountKey, AccountKey)
                };
                var result = translatorContainer.Translate(data[itemId].Dialog.Trim(), targetLanguage, sourceLanguage).Execute().ToList().FirstOrDefault();
                if (result == null)
                    return;

                data[itemId].Dialog = result.Text;

                LogWindow.AddMessage(LogType.Info, "Result: " + data[itemId].Dialog);
            }
            catch (Exception ex)
            {
                LogWindow.AddMessage(LogType.Error, ex.Message);
                if (ex.InnerException != null &&
                    ex.InnerException.Message.Contains("has an invalid pattern of characters"))
                {
                    LogWindow.AddMessage(LogType.Error, ex.InnerException.Message + " -> This language is not supported.");
                }
                StopTranslation = true;
            }
        }
    }
}