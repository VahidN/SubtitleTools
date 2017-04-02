using System;
using System.IO;
using System.Text;
using DNTPersianUtils.Core;
using SubtitleTools.Common.Regex;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.Core
{
    public class Sync
    {
        #region Methods (4)

        // Public Methods (4)

        public static void RecalculateRowNumbers(SubtitleItems data, string fileNameToSave)
        {
            if (data == null || string.IsNullOrWhiteSpace(fileNameToSave))
            {
                LogWindow.AddMessage(LogType.Alert, "Please open a file.");
                return;
            }

            //fix numbers
            for (var i = 0; i < data.Count; i++)
            {
                data[i].Number = i + 1;
            }

            //write data
            File.WriteAllText(fileNameToSave, ParseSrt.SubitemsToString(data).ApplyCorrectYeKe(), Encoding.UTF8);
            LogWindow.AddMessage(LogType.Info, "Finished RecalculateRowNumbers.");
        }

        public static string ShiftAllTimeLines(string[] lines, TimeSpan delta, bool add)
        {
            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                if (line.IsTimeLine())
                {
                    var times = RegexHelper.RgArrow.Split(line);

                    var stStart = times[0];
                    var stEnd = times[1];

                    var stNewTimeLine = string.Format("{0} --> {1}", ShiftThisTime(stStart, delta, add), ShiftThisTime(stEnd, delta, add));

                    sb.AppendLine(stNewTimeLine);

                }
                else
                    sb.AppendLine(line);
            }

            return sb.ToString();
        }

        public static void ShiftFileTimeLines(string srtFile, int hour, int minutes, int seconds, int milliseconds, bool add)
        {
            if (hour == 0 && minutes == 0 && seconds == 0 && milliseconds == 0)
                return;

            LogWindow.AddMessage(LogType.Info, "ShiftFileTimeLines Start.");

            //backup original file
            var backupFile = string.Format("{0}.bak", srtFile);
            File.Copy(srtFile, backupFile, overwrite: true);
            LogWindow.AddMessage(LogType.Info, string.Format("Backup file: {0}", backupFile));

            var delta = new TimeSpan(0, hour, minutes, seconds, milliseconds);
            var lines = File.ReadAllLines(srtFile);
            var newContent = ShiftAllTimeLines(lines, delta, add);
            File.WriteAllText(srtFile, newContent.ApplyCorrectYeKe(), Encoding.UTF8);

            LogWindow.AddMessage(LogType.Info, "ShiftFileTimeLines End.");
        }

        public static string ShiftThisTime(string line, TimeSpan delta, bool add)
        {
            var lineTs = ParseSrt.ConvertStringToTimeSpan(line);

            if (add) lineTs += delta;
            else lineTs -= delta;

            return ParseSrt.TimeSpanToString(lineTs);
        }

        #endregion Methods
    }
}
