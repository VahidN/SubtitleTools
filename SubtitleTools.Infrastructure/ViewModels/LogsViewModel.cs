using System.ComponentModel;
using System.Text;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Common.Toolkit;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class LogsViewModel : ViewModelBase
    {
        #region Fields (1)

        Log _selectedItem;

        #endregion Fields

        #region Constructors (1)

        public LogsViewModel()
        {
            setupCommands();
            LogItemsData = new Logs();
            App.Messenger.Register<Log>("AddLog", addLog);
        }

        #endregion Constructors

        #region Properties (5)

        public DelegateCommand<string> DoClearList { set; get; }

        public DelegateCommand<string> DoCopyAllLines { set; get; }

        public DelegateCommand<Log> DoCopySelectedLine { set; get; }

        public Logs LogItemsData { set; get; }

        public Log SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        #endregion Properties

        #region Methods (8)

        // Private Methods (8) 

        void addLog(Log log)
        {
            LogItemsData.Add(log);
        }

        static bool canDoClearList(string data)
        {
            return true;
        }

        static bool canDoCopyAllLines(string data)
        {
            return true;
        }

        static bool canDoCopySelectedLine(Log data)
        {
            return true;
        }

        void doClearList(string data)
        {
            LogItemsData.Clear();
        }

        void doCopyAllLines(string data)
        {
            var lines = new StringBuilder();
            foreach (var item in LogItemsData)
            {
                lines.AppendLine(string.Format("{0}\t{1}", item.Time, item.Message));
            }

            lines.ToString().ClipboardSetText();
        }

        void doCopySelectedLine(Log data)
        {
            if (SelectedItem == null) return;
            (string.Format("{0}\t{1}", SelectedItem.Time, SelectedItem.Message)).ClipboardSetText();
        }

        private void setupCommands()
        {
            DoCopySelectedLine = new DelegateCommand<Log>(doCopySelectedLine, canDoCopySelectedLine);
            DoCopyAllLines = new DelegateCommand<string>(doCopyAllLines, canDoCopyAllLines);
            DoClearList = new DelegateCommand<string>(doClearList, canDoClearList);
        }

        #endregion Methods
    }
}
