using System.IO;
using Microsoft.Win32;
using SubtitleTools.Infrastructure.Models;
using SubtitleTools.Common.Files;
using System.Text;
using DNTPersianUtils.Core;

namespace SubtitleTools.Infrastructure.Core
{
    public class Merge
    {
        #region Methods (3)

        // Public Methods (3)

        public static void MergeLists(SubtitleItems mainList, SubtitleItems timesFromList)
        {
            var mainListCount = mainList.Count;
            for (var i = 0; i < timesFromList.Count; i++)
            {
                if (i < mainListCount)
                {
                    mainList[i].Time = timesFromList[i].Time;
                }
                else
                    break;
            }
        }

        public static void StartInteractive(string mainFile)
        {
            var dlg = new OpenFileDialog
            {
                Filter = Filters.SrtFilter
            };

            var result = dlg.ShowDialog();
            if (result != true) return;
            WriteMergedList(mainFile, fromFilepath: dlg.FileName);
        }

        public static void WriteMergedList(string mainFilePath, string fromFilepath)
        {
            LogWindow.AddMessage(LogType.Info, "WriteMergedList Start.");
            //backup original file
            var backupFile = string.Format("{0}.original.bak", mainFilePath);
            File.Copy(mainFilePath, backupFile, overwrite: true);
            LogWindow.AddMessage(LogType.Info, string.Format("Backup file: {0}", backupFile));

            //read files
            var mainItems = new ParseSrt().ToObservableCollectionFromFile(mainFilePath);
            var fromItems = new ParseSrt().ToObservableCollectionFromFile(fromFilepath);

            //merge
            MergeLists(mainItems, fromItems);
            //toString
            var content = ParseSrt.SubitemsToString(mainItems);
            //write to file
            File.WriteAllText(mainFilePath, content.ApplyCorrectYeKe(), Encoding.UTF8);
            LogWindow.AddMessage(LogType.Info, "WriteMergedList End.");
        }

        #endregion Methods
    }
}
