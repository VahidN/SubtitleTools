using System;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Common.CodePlexRss.Model
{
    public class VersionInfo : ViewModelBase
    {
        #region Fields (5)

        string _author;
        string _description;
        string _link;
        DateTime _pubDate;
        string _title;

        #endregion Fields

        #region Properties (5)

        public string Author
        {
            set
            {
                if (_author == value) return;
                _author = value;
                RaisePropertyChanged("Author");
            }
            get { return _author; }
        }

        public string Description
        {
            set
            {
                if (_description == value) return;
                _description = value;
                RaisePropertyChanged("Description");
            }
            get { return _description; }
        }

        public string Link
        {
            set
            {
                if (_link == value) return;
                _link = value;
                RaisePropertyChanged("Link");
            }
            get { return _link; }
        }

        public DateTime PubDate
        {
            set
            {
                if (_pubDate == value) return;
                _pubDate = value;
                RaisePropertyChanged("PubDate");
            }
            get { return _pubDate; }
        }

        public string Title
        {
            set
            {
                if (_title == value) return;
                _title = value;
                RaisePropertyChanged("Title");
            }
            get { return _title; }
        }

        #endregion Properties
    }
}
