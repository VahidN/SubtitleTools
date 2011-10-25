using System.ComponentModel;
using SubtitleTools.Common.ISO639;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Infrastructure.Models
{
    public class DownloadOpenSubtitlesGui : ViewModelBase
    {
        #region Fields (4)

        bool _isBusy;
        string _moviePath;
        int _progress;
        Language _subLanguageId;
        OsdbItem _selectedOsdbItem;

        #endregion Fields

        #region Properties (4)

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

        public OsdbItem SelectedOsdbItem
        {
            get { return _selectedOsdbItem; }
            set
            {
                _selectedOsdbItem = value;
                RaisePropertyChanged("SelectedOsdbItem");
            }
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

        string _movieName;
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

        string _movieYear;
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

        string _imdbRating;
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

        string _osdbUrl;
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

        string _imdbUrl;
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

        #endregion Properties
    }
}
