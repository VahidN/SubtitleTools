using System.IO;
using SubtitleTools.Infrastructure.Models;
using SubtitleTools.Common.Toolkit;
using System.Text;

namespace SubtitleTools.Infrastructure.Core
{
    public class DeleteRow
    {
        #region Methods (1)

        // Public Methods (1) 

        public static void DeleteWholeRow(SubtitleItems data, SubtitleItem toDelete, string fileNameToSave)
        {
            if (data == null || toDelete == null || string.IsNullOrWhiteSpace(fileNameToSave))
            {
                LogWindow.AddMessage(LogType.Alert, "Please select a line to delete it.");
                return;
            }

            var number = toDelete.Number;
            if (data.Remove(toDelete))
            {
                //fix numbers
                for (var i = 0; i < data.Count; i++)
                {
                    data[i].Number = i + 1;
                }

                //backup original file
                var backupFile = string.Format("{0}.original.bak", fileNameToSave);
                File.Copy(fileNameToSave, backupFile, overwrite: true);

                //write data
                File.WriteAllText(fileNameToSave, ParseSrt.SubitemsToString(data).ApplyUnifiedYeKe(), Encoding.UTF8);

                LogWindow.AddMessage(LogType.Info, string.Format("Line {0} has been deleted.", number));
                LogWindow.AddMessage(LogType.Info, string.Format("Backup file: {0}", backupFile));
            }
            else
            {
                LogWindow.AddMessage(LogType.Alert, string.Format("Couldn't delete line {0}", number));
            }
        }

        #endregion Methods
    }
}
