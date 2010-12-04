using System.IO;
using System.Text;
using SubtitleTools.Common.Regex;

namespace SubtitleTools.Infrastructure.Core
{
    public class ChangeEncoding
    {
        #region Properties (1)

        public bool IsRtl { set; get; }

        #endregion Properties

        #region Methods (2)

        // Public Methods (2) 

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
            File.WriteAllText(path, data, Encoding.UTF8);
            //set flowDir
            IsRtl = File.ReadAllText(path).ContainsFarsi();
            LogWindow.AddMessage(LogType.Info, "ChangeEncoding End.");
            return true;
        }

        public bool IsUTF8(string filePath)
        {
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

        #endregion Methods
    }
}
