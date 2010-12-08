using System.ComponentModel;

namespace SubtitleTools.Common.EncodingHelper.Model
{
    public class EncodingInf : INotifyPropertyChanged
    {
        #region Fields (2)

        string _bodyName;
        string _name;

        #endregion Fields

        #region Properties (2)

        public string BodyName
        {
            set
            {
                if (_bodyName == value) return;
                _bodyName = value;
            }
            get { return _bodyName; }
        }

        public string Name
        {
            set
            {
                if (_name == value) return;
                _name = value;
            }
            get { return _name; }
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
