using System;
using System.IO;
using System.Linq;
using SubtitleTools.Common.Toolkit;
using SubtitleTools.Infrastructure.Models;
using System.Text;

namespace SubtitleTools.Infrastructure.Core
{
    public class MixFiles
    {
        public static void MixLists(SubtitleItems mainList, SubtitleItems secondList)
        {
            var mainListCount = mainList.Count;
            for (var i = 0; i < secondList.Count; i++)
            {
                if (i < mainListCount)
                {
                    var secondItem = secondList.Where(s => s.Time == mainList[i].Time).FirstOrDefault();
                    if (secondItem != null)
                    {
                        mainList[i].Dialog = mainList[i].Dialog + Environment.NewLine + secondItem.Dialog;
                    }
                }
                else
                    break;
            }
        }

        public static void WriteMixedList(string mainFilePath, string fromFilepath)
        {
            LogWindow.AddMessage(LogType.Info, "WriteMixedList Start.");
            //backup original file
            var backupFile = string.Format("{0}.original.bak", mainFilePath);
            File.Copy(mainFilePath, backupFile, overwrite: true);
            LogWindow.AddMessage(LogType.Info, string.Format("Backup file: {0}", backupFile));

            //read files
            var mainItems = new ParseSrt().ToObservableCollectionFromFile(mainFilePath);
            var fromItems = new ParseSrt().ToObservableCollectionFromFile(fromFilepath);

            //merge
            MixLists(mainItems, fromItems);
            //toString
            var content = ParseSrt.SubitemsToString(mainItems);
            //write to file
            File.WriteAllText(mainFilePath, content.ApplyUnifiedYeKe(), Encoding.UTF8);
            LogWindow.AddMessage(LogType.Info, "WriteMixedList End.");
        }
    }
}
