using System.Windows;

namespace SubtitleTools.Shell
{
    public partial class TranslatorView
    {
        public TranslatorView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FilePathProperty =
                        DependencyProperty.Register(
                                    "FilePath", typeof(string),
                                    typeof(TranslatorView),
                                    new PropertyMetadata(string.Empty));

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }
    }
}