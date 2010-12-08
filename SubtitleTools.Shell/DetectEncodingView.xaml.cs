using System.Windows;
using SubtitleTools.Infrastructure.ViewModels;

namespace SubtitleTools.Shell
{
    public partial class DetectEncodingView
    {
        DetectEncodingViewModel _vm;
        public DetectEncodingView()
        {
            InitializeComponent();
            _vm = new DetectEncodingViewModel();
            LayoutRoot.DataContext = _vm;
        }

        public static readonly DependencyProperty FilePathProperty =
                        DependencyProperty.Register(
                                    "FilePath", typeof(string),
                                    typeof(DetectEncodingView),
                                    new PropertyMetadata(string.Empty, onFilePathPropertyChanged));

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        private static void onFilePathPropertyChanged(DependencyObject d,
                                DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var detectEncodingView = d as DetectEncodingView;
                if (detectEncodingView != null)
                    detectEncodingView.listeningToDependencyPropertyChanges(e.NewValue.ToString());
            }
        }

        void listeningToDependencyPropertyChanges(string path)
        {            
            _vm.FilePath = path;
        }
    }
}
