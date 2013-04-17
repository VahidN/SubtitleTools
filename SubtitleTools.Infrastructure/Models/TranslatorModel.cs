using System.Collections.Generic;
using System.Linq;
using SubtitleTools.Common.ISO639;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Infrastructure.Models
{
    public class TranslatorModel : ViewModelBase
    {
        private Language _fromLanguage;
        private Language _toLanguage;

        public Language FromLanguage
        {
            get { return _fromLanguage; }
            set
            {
                if (_fromLanguage == value) return;
                _fromLanguage = value;
                RaisePropertyChanged("FromLanguage");
            }
        }

        public Language ToLanguage
        {
            get { return _toLanguage; }
            set
            {
                if (_toLanguage == value) return;
                _toLanguage = value;
                RaisePropertyChanged("ToLanguage");
            }
        }

        public IList<Language> SubLanguages
        {
            get
            {
                var lc = new LanguagesCodes();
                return lc.OrderBy(o => o.LanguageName).Skip(1).ToList();
            }
        }
    }
}