using System;
using System.IO;
using System.Linq;
using SubtitleTools.Infrastructure.Models;
using SubtitleTools.Common.Toolkit;

namespace SubtitleTools.Infrastructure.Core
{
    public class JoinFiles
    {
        #region Methods (4)

        // Public Methods (4) 

        public static Tuple<SubtitleItem, SubtitleItem, TimeSpan> DetectStartTimeOfSecondFile(string file1, string file2)
        {
            var firstFileLastItem = SubtitleFileLastItem(file1);
            LogWindow.AddMessage(LogType.Info,
                string.Format("FirstFileLastTs: {0}:{1}:{2},{3}", firstFileLastItem.EndTs.Hours, firstFileLastItem.EndTs.Minutes, firstFileLastItem.EndTs.Seconds, firstFileLastItem.EndTs.Milliseconds));

            var secondFileFirstItem = SubtitleFileFirstItem(file2);
            LogWindow.AddMessage(LogType.Info,
                string.Format("SecondFileFirstTs: {0}:{1}:{2},{3}", secondFileFirstItem.StartTs.Hours, secondFileFirstItem.StartTs.Minutes, secondFileFirstItem.StartTs.Seconds, secondFileFirstItem.StartTs.Milliseconds));

            var startTs = firstFileLastItem.EndTs + secondFileFirstItem.StartTs;
            LogWindow.AddMessage(LogType.Info,
                string.Format("Possible StartTs: {0}:{1}:{2},{3}", startTs.Hours, startTs.Minutes, startTs.Seconds, startTs.Milliseconds));

            return new Tuple<SubtitleItem, SubtitleItem, TimeSpan>(firstFileLastItem, secondFileFirstItem, startTs);
        }

        public static void JoinTwoFiles(string file1, string file2, TimeSpan file2StartTsSelectedByUser)
        {
            LogWindow.AddMessage(LogType.Info, "JoinTwoFiles Start");
            var file1Items = new ParseSrt().ToObservableCollectionFromFile(file1);
            var file2Items = new ParseSrt().ToObservableCollectionFromFile(file2);

            var file1LastItem = file1Items.OrderByDescending(o => o.Number).FirstOrDefault();
            var file2FirstItem = file2Items.OrderBy(o => o.Number).FirstOrDefault();
            var delta = file2StartTsSelectedByUser - file2FirstItem.StartTs;

            for (int i = 0; i < file2Items.Count; i++)
            {
                //Correct file2's numbers
                file2Items[i].Number = i + 1 + file1LastItem.Number;
                //shift file2's items
                file2Items[i].StartTs += delta;
                file2Items[i].EndTs += delta;
                //write it back
                file2Items[i].Time =
                    string.Format("{0} --> {1}", ParseSrt.TimeSpanToString(file2Items[i].StartTs), ParseSrt.TimeSpanToString(file2Items[i].EndTs));
            }

            var newShiftedFile2 = ParseSrt.SubitemsToString(file2Items);

            //Create a single file
            var fileName = string.Format("{0}\\JoinedFile{1}", Path.GetDirectoryName(file1), Path.GetExtension(file1));
            var file1Content = File.ReadAllText(file1);
            File.WriteAllText(fileName, file1Content.ApplyUnifiedYeKe());
            File.AppendAllText(fileName, newShiftedFile2.ApplyUnifiedYeKe());
            LogWindow.AddMessage(LogType.Announcement, string.Format("Saved to:  {0}", fileName));
        }

        public static SubtitleItem SubtitleFileFirstItem(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var firstSubtitleItems = new ParseSrt().ToObservableCollectionFromContent(content);

            var firstItem = firstSubtitleItems.OrderBy(o => o.Number).FirstOrDefault();
            if (firstItem == null) throw new InvalidDataException("Please select a valid subtitle file.");

            return new SubtitleItem
            {
                Dialog = firstItem.Dialog,
                Number = firstItem.Number,
                Time = firstItem.Time,
                EndTs = firstItem.EndTs,
                StartTs = firstItem.StartTs
            };
        }

        public static SubtitleItem SubtitleFileLastItem(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var firstSubtitleItems = new ParseSrt().ToObservableCollectionFromContent(content);

            var lastItem = firstSubtitleItems.OrderByDescending(o => o.Number).FirstOrDefault();
            if (lastItem == null) throw new InvalidDataException("Please select a valid subtitle file.");

            return new SubtitleItem
            {
                Dialog = lastItem.Dialog,
                Number = lastItem.Number,
                Time = lastItem.Time,
                EndTs = lastItem.EndTs,
                StartTs = lastItem.StartTs
            };
        }

        #endregion Methods
    }
}
