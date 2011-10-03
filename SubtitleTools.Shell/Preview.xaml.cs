using System;
using System.Windows;

namespace SubtitleTools.Shell
{
    public partial class Preview
    {
        public Preview()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FilePathProperty =
                        DependencyProperty.Register(
                                    "FilePath", typeof(Uri),
                                    typeof(Preview),
                                    new PropertyMetadata(null));

        public Uri FilePath
        {
            get { return (Uri)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }
    }
}
