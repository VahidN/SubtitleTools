using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DNTPersianUtils.Core;
using Microsoft;
using SubtitleTools.Common.ISO639;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.Core
{
    public class Translate
    {
        private const string AccountKey = "PKWTH4J8g2xmPGTHmQOAlyQsi/aJmef+u5iiuu7rwjo=";
        private readonly Uri ServiceUri = new Uri("https://api.datamarket.azure.com/Bing/MicrosoftTranslator/");
        private bool _isRunning;
        private string _sourceLanguage;
        private string _targetLanguage;
        private SubtitleItems _data;

        public IList<SubtitleTools.Common.ISO639.Language> GetSupportedLanguages()
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

        public bool StopTranslation { set; get; }

        public void TranslateAll(string fileName, string sourceLanguage, string targetLanguage)
        {
            if (_isRunning)
            {
                LogWindow.AddMessage(LogType.Alert, "Translation is in progress...");
                return;
            }
            _isRunning = true;
            StopTranslation = false;

            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    LogWindow.AddMessage(LogType.Alert, "Please open a file.");
                    return;
                }

                _data = new ParseSrt().ToObservableCollectionFromFile(fileName);
                if (_data == null || !_data.Any())
                {
                    LogWindow.AddMessage(LogType.Alert, "Please open a file.");
                    return;
                }

                _sourceLanguage = sourceLanguage;
                _targetLanguage = targetLanguage;

                for (var itemId = 0; itemId < _data.Count; itemId++)
                {
                    if (StopTranslation)
                        break;
                    translateItem(itemId);
                }

                var saveToFileName = getSaveToFileName(fileName);
                File.WriteAllText(saveToFileName, ParseSrt.SubitemsToString(_data).ApplyCorrectYeKe(), Encoding.UTF8);
                LogWindow.AddMessage(LogType.Info, "Finished Translating. Saved to " + saveToFileName);
            }
            finally
            {
                _isRunning = false;
            }
        }

        private string getSaveToFileName(string fileName)
        {
            var saveToFileName = Path.GetFileNameWithoutExtension(fileName) + "." + _targetLanguage + Path.GetExtension(fileName);
            return Path.Combine(Path.GetDirectoryName(fileName), saveToFileName);
        }

        private void translateItem(int itemId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_data[itemId].Dialog))
                    return;

                LogWindow.AddMessage(LogType.Info, "Translating: " + _data[itemId].Dialog);

                var translatorContainer = new TranslatorContainer(ServiceUri)
                {
                    Credentials = new NetworkCredential(AccountKey, AccountKey)
                };
                var result = translatorContainer.Translate(_data[itemId].Dialog.Trim(), _targetLanguage, _sourceLanguage).Execute().ToList().FirstOrDefault();
                if (result == null)
                    return;

                _data[itemId].Dialog = result.Text;

                LogWindow.AddMessage(LogType.Info, "Result: " + _data[itemId].Dialog);
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