using System.Windows;

namespace SubtitleTools.Shell
{
    public partial class SyncView
    {
        public SyncView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FilePathProperty =
                        DependencyProperty.Register(
                                    "FilePath", typeof(string),
                                    typeof(SyncView),
                                    new PropertyMetadata(string.Empty));

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }
    }
}
