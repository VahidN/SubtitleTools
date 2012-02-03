using SubtitleTools.Common.ISO639;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Infrastructure.Models
{
    public class DownloadOpenSubtitlesGui : ViewModelBase
    {
        #region Fields (10)

        string _imdbRating;
        string _imdbUrl;
        bool _isBusy;
        string _movieName;
        string _moviePath;
        string _movieYear;
        string _osdbUrl;
        int _progress;
        OsdbItem _selectedOsdbItem;
        Language _subLanguageId;

        #endregion Fields

        #region Properties (10)

        public string ImdbRating
        {
            set
            {
                if (_imdbRating == value) return;
                _imdbRating = value;
                RaisePropertyChanged("ImdbRating");
            }
            get { return _imdbRating; }
        }

        public string ImdbUrl
        {
            set
            {
                if (_imdbUrl == value) return;
                _imdbUrl = value;
                RaisePropertyChanged("ImdbUrl");
            }
            get { return _imdbUrl; }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public string MovieName
        {
            set
            {
                if (_movieName == value) return;
                _movieName = value;
                RaisePropertyChanged("MovieName");
            }
            get { return _movieName; }
        }

        public string MoviePath
        {
            set
            {
                if (_moviePath == value) return;
                _moviePath = value;
                RaisePropertyChanged("MoviePath");
            }
            get { return _moviePath; }
        }

        public string MovieYear
        {
            set
            {
                if (_movieYear == value) return;
                _movieYear = value;
                RaisePropertyChanged("MovieYear");
            }
            get { return _movieYear; }
        }

        public string OsdbUrl
        {
            set
            {
                if (_osdbUrl == value) return;
                _osdbUrl = value;
                RaisePropertyChanged("OsdbUrl");
            }
            get { return _osdbUrl; }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                if (_progress == value) return;
                _progress = value;
                RaisePropertyChanged("Progress");
            }
        }

        public OsdbItem SelectedOsdbItem
        {
            get { return _selectedOsdbItem; }
            set
            {
                _selectedOsdbItem = value;
                RaisePropertyChanged("SelectedOsdbItem");
            }
        }

        public Language SubLanguage
        {
            set
            {
                if (_subLanguageId == value) return;
                _subLanguageId = value;
                RaisePropertyChanged("SubLanguageId");
            }
            get { return _subLanguageId; }
        }

        #endregion Properties
    }
}
