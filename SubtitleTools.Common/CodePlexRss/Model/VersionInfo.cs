using System;
using System.ComponentModel;

namespace SubtitleTools.Common.CodePlexRss.Model
{
    public class VersionInfo : INotifyPropertyChanged
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
                raisePropertyChanged("Author");
            }
            get { return _author; }
        }

        public string Description
        {
            set
            {
                if (_description == value) return;
                _description = value;
                raisePropertyChanged("Description");
            }
            get { return _description; }
        }

        public string Link
        {
            set
            {
                if (_link == value) return;
                _link = value;
                raisePropertyChanged("Link");
            }
            get { return _link; }
        }

        public DateTime PubDate
        {
            set
            {
                if (_pubDate == value) return;
                _pubDate = value;
                raisePropertyChanged("PubDate");
            }
            get { return _pubDate; }
        }

        public string Title
        {
            set
            {
                if (_title == value) return;
                _title = value;
                raisePropertyChanged("Title");
            }
            get { return _title; }
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
