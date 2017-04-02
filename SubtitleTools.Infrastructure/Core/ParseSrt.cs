using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DNTPersianUtils.Core;
using Microsoft.Win32;
using SubtitleTools.Common.Files;
using SubtitleTools.Common.Regex;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.Core
{
    using System.Globalization;

    public class ParseSrt
    {
        #region Properties (2)

        public string FilePath { set; get; }

        public bool IsRtl { set; get; }

        #endregion Properties

        #region Methods (11)

        // Public Methods (11) 

        public static SubtitleItems AddSubtitleItemToList(SubtitleItems subtitleItemsDataInternal, SubtitleItem subtitleItem)
        {
            var localSubItems = subtitleItemsDataInternal.ToList();

            var newItem = new SubtitleItem
            {
                Dialog = subtitleItem.Dialog,
                EndTs = subtitleItem.EndTs,
                StartTs = subtitleItem.StartTs
            };

            localSubItems.Add(newItem);
            localSubItems = localSubItems.OrderBy(x => x.StartTs).ToList();

            var i = 1;
            var finalItems = new SubtitleItems();
            foreach (var item in localSubItems)
            {
                item.Number = i++;
                finalItems.Add(item);
            }

            return finalItems;
        }

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

        public static IList<string> FindConflicts(SubtitleItems subtitleItems)
        {
            var result = new List<string>();

            for (var i = 1; i < subtitleItems.Count; i++)
            {
                if (subtitleItems[i].StartTs < subtitleItems[i - 1].EndTs)
                    result.Add(string.Format("Conflict in #{0}: Start({1})<End({2})", i, (i + 1), (i)));
            }
            return result;
        }

        public static SubtitleItem GetCurrentSubtile(SubtitleItems subtitleItems, TimeSpan currentMediaPosition)
        {
            if (subtitleItems == null || !subtitleItems.Any()) return null;
            return subtitleItems.Where(x => x.StartTs <= currentMediaPosition &&
                                            x.EndTs >= currentMediaPosition)
                                        .OrderByDescending(x => x.Number)
                                        .FirstOrDefault();
        }

        public static string SetMaxWordsPerLine(string dialog)
        {
            if (string.IsNullOrWhiteSpace(dialog)) return string.Empty;

            var lines = dialog.Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                var words = lines[i].Trim().Split(new[] { " " }, StringSplitOptions.None);
                if (words.Length > 6)
                {
                    var count = 1;
                    var result = new StringBuilder();

                    foreach (var word in words)
                    {
                        result.Append(word);
                        result.Append(count % 7 == 0 ? Environment.NewLine : " ");

                        count++;
                    }

                    lines[i] = result.ToString().Trim();
                }
            }

            var finalResult = new StringBuilder();
            foreach (var line in lines)
                finalResult.AppendLine(line);

            return finalResult.ToString().Trim();
        }

        public static string SubitemsToString(SubtitleItems mainItems)
        {
            var sb = new StringBuilder();
            var i = 1;
            foreach (var item in mainItems.OrderBy(x => x.StartTs))
            {
                item.FixMinReadTime();

                sb.AppendLine(i.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine(item.Time);
                sb.AppendLine(item.Dialog.Trim());
                sb.AppendLine(string.Empty);
                i++;
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

            var res = new List<SubtitleItem>();

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

            var result = new SubtitleItems();
            var i = 1;
            foreach (var item in res.OrderBy(x => x.StartTs))
            {
                item.Number = i++;
                result.Add(item);
            }

            LogWindow.AddMessage(LogType.Info, "ParseSrt End.");
            return result;
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
