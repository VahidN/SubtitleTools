using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Infrastructure.Models
{
    public class SpeechRecognitionEngine : ViewModelBase
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
        }
    }
}