using System.ComponentModel;
using SubtitleTools.Common.ISO639;

namespace SubtitleTools.Infrastructure.Models
{
    public class UploadItem : INotifyPropertyChanged
    {
        #region Fields (6)

        string _finalUrl;
        bool _isBusy;
        string _moviePath;
        int _progress;
        Language _selectedSubtitleLanguage;
        string _subtitlePath;
        long _imdbId;

        #endregion Fields

        #region Properties (6)

        public string FinalUrl
        {
            get { return _finalUrl; }
            set
            {
                if (_finalUrl == value) return;
                _finalUrl = value;
                raisePropertyChanged("FinalUrl");
            }
        }

        public long ImdbId
        {
            get { return _imdbId; }
            set
            {
                if (_imdbId == value) return;
                _imdbId = value;
                raisePropertyChanged("ImdbId");
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                raisePropertyChanged("IsBusy");
            }
        }

        public string MoviePath
        {
            get { return _moviePath; }
            set
            {
                if (_moviePath == value) return;
                _moviePath = value;
                raisePropertyChanged("MoviePath");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                if (_progress == value) return;
                _progress = value;
                raisePropertyChanged("Progress");
            }
        }

        public Language SelectedSubtitleLanguage
        {
            get { return _selectedSubtitleLanguage; }
            set
            {
                if (_selectedSubtitleLanguage == value) return;
                _selectedSubtitleLanguage = value;
                raisePropertyChanged("SelectedSubtitleLanguage");
            }
        }

        public string SubtitlePath
        {
            get { return _subtitlePath; }
            set
            {
                if (_subtitlePath == value) return;
                _subtitlePath = value;
                raisePropertyChanged("SubtitlePath");
            }
        }

        #endregion Properties



        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        void raisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
