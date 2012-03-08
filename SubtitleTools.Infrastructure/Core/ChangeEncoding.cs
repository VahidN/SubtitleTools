using System.IO;
using System.Text;
using href.Utils;
using SubtitleTools.Common.Regex;
using SubtitleTools.Common.Toolkit;

namespace SubtitleTools.Infrastructure.Core
{
    public class ChangeEncoding
    {
        #region Properties (1)

        public bool IsRtl { set; get; }

        #endregion Properties

        #region Methods (4)

        // Public Methods (4) 

        public bool FixWindows1256(string path)
        {
            LogWindow.AddMessage(LogType.Info, "ChangeEncoding Start.");
            if (IsUTF8(path))
            {
                LogWindow.AddMessage(LogType.Alert, "This file IsUTF8!");
                return false;
            }

            //backup original file
            var backupFile = string.Format("{0}.bak", path);
            File.Copy(path, backupFile, overwrite: true);
            LogWindow.AddMessage(LogType.Info, string.Format("Backup file: {0}", backupFile));

            //convert
            var data = File.ReadAllText(path, Encoding.GetEncoding("windows-1256"));
            File.WriteAllText(path, data.ApplyUnifiedYeKe(), Encoding.UTF8);
            //set flowDir
            IsRtl = File.ReadAllText(path).ContainsFarsi();
            LogWindow.AddMessage(LogType.Info, "ChangeEncoding End.");
            return true;
        }

        public bool IsUTF8(string filePath)
        {
            var mostEfficientEncoding = EncodingTools.DetectInputCodepage(File.ReadAllBytes(filePath));
            if (mostEfficientEncoding.CodePage == 65001)
                return true;

            using (var file = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (file.CanSeek)
                {
                    var bom = new byte[4]; // Get the byte-order mark, if there is one
                    file.Read(bom, 0, 4);
                    return (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf);
                }
                //it's a binary file
                return false;
            }
        }

        public bool ToUTF8(string path, string fromEnc)
        {
            LogWindow.AddMessage(LogType.Info, string.Format("ChangeEncoding ({0}) Start.", path));
            if (IsUTF8(path))
            {
                LogWindow.AddMessage(LogType.Alert, "This file IsUTF8!");
                return false;
            }

            //backup original file
            var backupFile = string.Format("{0}.bak", path);
            File.Copy(path, backupFile, overwrite: true);
            LogWindow.AddMessage(LogType.Info, string.Format("Backup file: {0}", backupFile));

            //convert
            var data = File.ReadAllText(path, Encoding.GetEncoding(fromEnc));
            File.WriteAllText(path, data.ApplyUnifiedYeKe(), Encoding.UTF8);
            //set flowDir
            IsRtl = File.ReadAllText(path).ContainsFarsi();
            LogWindow.AddMessage(LogType.Info, "ChangeEncoding End.");
            return true;
        }

        public void ConvertAllFilesEncodings(string folder, string fromEnc)
        {
            foreach (var file in Directory.GetFiles(folder, "*.srt"))
            {
                ToUTF8(file, fromEnc);
            }
        }

        public string TryReduceRtlLargeFileContent(string fileName)
        {
            // OSDB's PHP server can't accept subtitle files larger than 100KB!
            // LTR languages are fine (most of the times), but RTL languages with utf-8 encoding have problems,
            // because utf-8 means larger files than original windows-1256 files.               
            if (new FileInfo(fileName).Length < 102400)
            {
                //it's fine for OSDB upload
                return fileName;
            }

            var content = File.ReadAllText(fileName);
            if (!content.ContainsFarsi())
            {
                //it's not RTL
                return fileName;
            }

            var mostEfficientEncoding = EncodingTools.DetectInputCodepage(File.ReadAllBytes(fileName));
            if (mostEfficientEncoding.CodePage != 65001)
            {
                //don't corrupt it.
                return fileName;
            }

            var newFilePath = string.Format("{0}\\sub-{1}", Path.GetDirectoryName(fileName), Path.GetFileName(fileName));
            File.WriteAllText(newFilePath, content, Encoding.GetEncoding("windows-1256"));
            LogWindow.AddMessage(LogType.Info, "Saved a new file with windows-1256 encoding to reduce the size of the sub file @ " + newFilePath);
            return newFilePath;
        }

        #endregion Methods
    }
}
