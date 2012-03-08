using System.Windows;
using System.Windows.Forms;
using System.Windows.Interactivity;

namespace SubtitleTools.Common.Behaviors
{
    public class FolderBrowserDialogBehavior : TargetedTriggerAction<System.Windows.Controls.Button>
    {
        #region Fields

        public static readonly DependencyProperty FolderBrowserDescriptionProperty =
           DependencyProperty.Register("FolderBrowserDescription", typeof(string),
           typeof(FolderBrowserDialogBehavior), null);

        public static readonly DependencyProperty FolderBrowserDialogResultCommandProperty =
            DependencyProperty.Register("FolderBrowserDialogResultCommand",
            typeof(object), typeof(FolderBrowserDialogBehavior), null);

        #endregion Fields

        #region Properties

        public string FolderBrowserDescription
        {
            get { return (string)GetValue(FolderBrowserDescriptionProperty); }
            set { SetValue(FolderBrowserDescriptionProperty, value); }
        }

        public object FolderBrowserDialogResultCommand
        {
            get { return GetValue(FolderBrowserDialogResultCommandProperty); }
            set { SetValue(FolderBrowserDialogResultCommandProperty, value); }
        }

        #endregion Properties

        #region Methods (1)

        // Protected Methods (1) 

        protected override void Invoke(object parameter)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog { ShowNewFolderButton = true })
            {
                if (!string.IsNullOrEmpty(FolderBrowserDescription))
                {
                    folderBrowserDialog.Description = FolderBrowserDescription;
                }

                var result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK)
                    FolderBrowserDialogResultCommand = folderBrowserDialog.SelectedPath;
            }
        }

        #endregion Methods
    }
}