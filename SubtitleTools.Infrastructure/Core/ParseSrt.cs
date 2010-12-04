using System;
using System.IO;
using System.Text;
using Microsoft.Win32;
using SubtitleTools.Common.Files;
using SubtitleTools.Common.Regex;
using SubtitleTools.Infrastructure.Models;
using System.Collections.Generic;

namespace SubtitleTools.Infrastructure.Core
{
    public class ParseSrt
    {
        #region Properties (2)

        public string FilePath { set; get; }

        public bool IsRtl { set; get; }

        #endregion Properties

        #region Methods (7)

        // Public Methods (7) 

        public static TimeSpan ConvertStringToTimeSpan(string line)
        {
            var m = RegexHelper.RgGroups.Match(line);

            var hh = int.Parse(m.Groups[1].ToString());
            var mm = int.Parse(m.Groups[2].ToString());
            var ss = int.Parse(m.Groups[3].ToString());
            var ff = int.Parse(m.Groups[4].ToString());

            var lineTs = new TimeSpan(0, hh, mm, ss, ff);
            return lineTs;
        }

        public static Tuple<TimeSpan, TimeSpan> ExtractTsStartEnd(string wholeTimeLine)
        {
            var times = RegexHelper.RgArrow.Split(wholeTimeLine);
            var stStart = times[0];
            var tsStart = ConvertStringToTimeSpan(stStart);
            var stEnd = times[1];
            var tsEnd = ConvertStringToTimeSpan(stEnd);
            return new Tuple<TimeSpan, TimeSpan>(tsStart, tsEnd);
        }

        public static string SubitemsToString(SubtitleItems mainItems)
        {
            var sb = new StringBuilder();
            foreach (var item in mainItems)
            {
                sb.AppendLine(item.Number.ToString());
                sb.AppendLine(item.Time);
                sb.AppendLine(item.Dialog.Trim());
                sb.AppendLine(string.Empty);
            }
            return sb.ToString();
        }

        public static string TimeSpanToString(TimeSpan lineTs)
        {
            return string.Format("{0}:{1}:{2},{3}", lineTs.Hours.ToString("D2"), lineTs.Minutes.ToString("D2"), lineTs.Seconds.ToString("D2"), lineTs.Milliseconds.ToString("D3"));
        }

        public SubtitleItems ToObservableCollectionFromContent(string srtFileContent)
        {
            LogWindow.AddMessage(LogType.Info, "ParseSrt Start.");

            IsRtl = srtFileContent.ContainsFarsi();

            var res = new SubtitleItems();

            var lines = srtFileContent.Split('\n');

            var dialog = string.Empty;
            var time = string.Empty;
            var number = string.Empty;

            foreach (var line in lines)
            {
                var data = line.Trim();
                if (string.IsNullOrWhiteSpace(data))
                {
                    if (!string.IsNullOrWhiteSpace(number)
                        && !string.IsNullOrWhiteSpace(time))
                    {
                        var ts = ExtractTsStartEnd(time);

                        res.Add(
                            new SubtitleItem
                            {
                                Dialog = dialog.Trim(),
                                Number = int.Parse(number),
                                Time = time,
                                StartTs = ts.Item1,
                                EndTs = ts.Item2
                            });
                        //for the next item
                        dialog = string.Empty;
                        time = string.Empty;
                        number = string.Empty;
                    }

                    continue;
                }

                if (data.IsNumeric() && time == string.Empty)
                {
                    number = data;
                }
                else if (data.IsTimeLine())
                {
                    time = data;
                }
                else
                {
                    dialog += data + Environment.NewLine;
                }
            }

            if (!string.IsNullOrWhiteSpace(number)
                        && !string.IsNullOrWhiteSpace(time))
            {
                var ts = ExtractTsStartEnd(time);
                res.Add(
                    new SubtitleItem
                    {
                        Dialog = dialog.Trim(),
                        Number = int.Parse(number),
                        Time = time,
                        StartTs = ts.Item1,
                        EndTs = ts.Item2
                    });
            }

            LogWindow.AddMessage(LogType.Info, "ParseSrt End.");
            return res;
        }

        public SubtitleItems ToObservableCollectionFromFile(string filePath)
        {
            var srtFileContent = File.ReadAllText(filePath);
            return ToObservableCollectionFromContent(srtFileContent);
        }

        public SubtitleItems ToObservableCollectionInteractive()
        {
            var dlg = new OpenFileDialog
            {
                Filter = Filters.SrtFilter
            };

            var result = dlg.ShowDialog();

            if (result != true) return new SubtitleItems();

            FilePath = dlg.FileName;
            var srtFileContent = File.ReadAllText(FilePath);
            return ToObservableCollectionFromContent(srtFileContent);
        }

        #endregion Methods
    }
}
