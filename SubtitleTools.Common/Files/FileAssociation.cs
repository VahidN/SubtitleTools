using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using SubtitleTools.Common.Logger;

namespace SubtitleTools.Common.Files
{
    //from : http://www.devx.com/vb2themax/Tip/19554?type=kbArticle&trk=MSCP
    public class FileAssociation
    {
        #region Fields (2)

        const int ShcneAssocchanged = 0x8000000;
        const int ShcnfIdlist = 0;

        #endregion Fields

        #region Methods (3)

        // Public Methods (3) 

        public static void CreateFileAssociation(
            string extension,
            string className,
            string description,
             string exeProgram)
        {
            // ensure that there is a leading dot
            if (extension.Substring(0, 1) != ".")
                extension = string.Format(".{0}", extension);

            try
            {
                if (IsAssociated(extension)) return;

                // create a value for this key that contains the classname
                using (var key1 = Registry.ClassesRoot.CreateSubKey(extension))
                {
                    if (key1 != null)
                    {
                        key1.SetValue("", className);
                        // create a new key for the Class name
                        using (var key2 = Registry.ClassesRoot.CreateSubKey(className))
                        {
                            if (key2 != null)
                            {
                                key2.SetValue("", description);
                                // associate the program to open the files with this extension
                                using (var key3 = Registry.ClassesRoot.CreateSubKey(string.Format(@"{0}\Shell\Open\Command", className)))
                                {
                                    if (key3 != null) key3.SetValue("", string.Format(@"{0} ""%1""", exeProgram));
                                }
                            }
                        }
                    }
                }

                // notify Windows that file associations have changed
                SHChangeNotify(ShcneAssocchanged, ShcnfIdlist, 0, 0);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is denied"))
                {
                    //It's the Windows Vista+ or a non admin user.
                    return;
                }
                ExceptionLogger.LogExceptionToFile(ex);
            }
        }

        // Return true if extension already associated in registry
        public static bool IsAssociated(string extension)
        {
            return (Registry.ClassesRoot.OpenSubKey(extension, false) != null);
        }

        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(int wEventId, int uFlags, int dwItem1, int dwItem2);

        #endregion Methods
    }
}