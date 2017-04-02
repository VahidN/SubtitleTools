using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Common.EncodingHelper.Model
{
    public class EncodingInf : ViewModelBase
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
                RaisePropertyChanged("BodyName");
            }
            get { return _bodyName; }
        }

        public string Name
        {
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged("Name");
            }
            get { return _name; }
        }

        #endregion Properties
    }
}
