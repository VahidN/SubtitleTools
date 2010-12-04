using System.ComponentModel;

namespace SubtitleTools.Infrastructure.Models
{
    public class OsdbItem : INotifyPropertyChanged
    {
        #region Fields (7)

        int _iDSubtitleFile;
        bool _isSelected;
        string _iso639;
        string _iso6393166_1;
        string _languageName;
        int _progress;
        string _subAddDate;
        string _subFileName;
        string _subSize;

        #endregion Fields

        #region Properties (7)

        public int IDSubtitleFile
        {
            set
            {
                if (_iDSubtitleFile == value) return;
                _iDSubtitleFile = value;
                raisePropertyChanged("IDSubtitleFile");
            }
            get { return _iDSubtitleFile; }
        }
        
        public string ISO639
        {
            set
            {
                if (_iso639 == value) return;
                _iso639 = value;
                raisePropertyChanged("ISO639");
            }
            get { return _iso639; }
        }
                
        public string ISO6393166_1
        {
            get { return _iso6393166_1; }
            set
            {
                if (_iso6393166_1 == value) return;
                _iso6393166_1 = value;
                raisePropertyChanged("ISO6393166_1");
            }
        }

        public bool IsSelected
        {
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                raisePropertyChanged("IsSelected");
            }
            get { return _isSelected; }
        }

        public string LanguageName
        {
            set
            {
                if (_languageName == value) return;
                _languageName = value;
                raisePropertyChanged("LanguageName");
            }
            get { return _languageName; }
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

        public string SubAddDate
        {
            set
            {
                if (_subAddDate == value) return;
                _subAddDate = value;
                raisePropertyChanged("SubAddDate");
            }
            get { return _subAddDate; }
        }

        public string SubFileName
        {
            set
            {
                if (_subFileName == value) return;
                _subFileName = value;
                raisePropertyChanged("SubFileName");
            }
            get { return _subFileName; }
        }

        public string SubSize
        {
            set
            {
                if (_subSize == value) return;
                _subSize = value;
                raisePropertyChanged("SubSize");
            }
            get { return _subSize; }
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
