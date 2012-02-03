using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.Win32;
using SubtitleTools.Common.Files;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Common.Behaviors
{
    public class OpenFileDialogBoxBehavior : TargetedTriggerAction<Button>
    {
        #region Fields (4)

        public static readonly DependencyProperty DialogFilterProperty =
           DependencyProperty.Register("DialogFilter", typeof(string),
           typeof(OpenFileDialogBoxBehavior), null);
        public static readonly DependencyProperty DialogFilterTypeProperty =
           DependencyProperty.Register("DialogFilterType", typeof(string),
           typeof(OpenFileDialogBoxBehavior), null);
        public static readonly DependencyProperty FileDialogDialogResultCommandProperty =
            DependencyProperty.Register("FileDialogDialogResultCommand",
            typeof(object), typeof(OpenFileDialogBoxBehavior), null);

        #endregion Fields

        #region Properties (3)

        public string DialogFilter
        {
            get { return (string)GetValue(DialogFilterProperty); }
            set { SetValue(DialogFilterProperty, value); }
        }

        public string DialogFilterType
        {
            get { return (string)GetValue(DialogFilterTypeProperty); }
            set { SetValue(DialogFilterTypeProperty, value); }
        }

        public object FileDialogDialogResultCommand
        {
            get { return GetValue(FileDialogDialogResultCommandProperty); }
            set { SetValue(FileDialogDialogResultCommandProperty, value); }
        }

        #endregion Properties

        #region Methods (1)

        // Protected Methods (1) 

        protected override void Invoke(object parameter)
        {
            var objOpenFileDialog = new OpenFileDialog();

            if (!string.IsNullOrEmpty(DialogFilter))
            {
                objOpenFileDialog.Filter = DialogFilter;
            }
            else if (!string.IsNullOrEmpty(DialogFilterType))
            {
                switch (DialogFilterType)
                {
                    case "Image":
                        objOpenFileDialog.Filter = Filters.ImageFilter;
                        break;
                    case "Movie":
                        objOpenFileDialog.Filter = Filters.MovieFilter;
                        break;
                    case "Subtitle":
                        objOpenFileDialog.Filter = Filters.SrtFilter;
                        break;
                    case "Wav":
                        objOpenFileDialog.Filter = Filters.WavFilter;
                        break;
                }
            }

            if (string.IsNullOrEmpty(objOpenFileDialog.Filter))
                objOpenFileDialog.Filter = Filters.ImageFilter;

            if (!string.IsNullOrWhiteSpace(App.StartupFileName))
                objOpenFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(App.StartupFileName);

            var result = objOpenFileDialog.ShowDialog();
            if (!result.Value) return;

            FileDialogDialogResultCommand = objOpenFileDialog.FileName;
        }

        #endregion Methods
    }
}
