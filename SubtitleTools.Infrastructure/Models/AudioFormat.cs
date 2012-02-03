using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Infrastructure.Models
{
    public class AudioFormat : ViewModelBase
    {
        #region Fields (5)

        int _bitsPerSample;
        int _blockAlign;
        int _channelCount;
        string _encodingformat;
        int _samplesPerSecond;

        #endregion Fields

        #region Properties (5)

        public int BitsPerSample
        {
            get { return _bitsPerSample; }
            set
            {
                _bitsPerSample = value;
                RaisePropertyChanged("BitsPerSample");
            }
        }

        public int BlockAlign
        {
            get { return _blockAlign; }
            set
            {
                _blockAlign = value;
                RaisePropertyChanged("BlockAlign");
            }
        }

        public int ChannelCount
        {
            get { return _channelCount; }
            set
            {
                _channelCount = value;
                RaisePropertyChanged("ChannelCount");
            }
        }

        public string Encodingformat
        {
            get { return _encodingformat; }
            set
            {
                _encodingformat = value;
                RaisePropertyChanged("Encodingformat");
            }
        }

        public int SamplesPerSecond
        {
            get { return _samplesPerSecond; }
            set
            {
                _samplesPerSecond = value;
                RaisePropertyChanged("SamplesPerSecond");
            }
        }

        #endregion Properties
    }
}