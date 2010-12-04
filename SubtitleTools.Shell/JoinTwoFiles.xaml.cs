using System.Windows;

namespace SubtitleTools.Shell
{
    public partial class JoinTwoFiles
    {
        public JoinTwoFiles()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FirstFilePathProperty =
                        DependencyProperty.Register(
                                    "FirstFilePath", typeof(string),
                                    typeof(JoinTwoFiles),
                                    new PropertyMetadata(string.Empty));

        public string FirstFilePath
        {
            get { return (string)GetValue(FirstFilePathProperty); }
            set { SetValue(FirstFilePathProperty, value); }
        }
    }
}
