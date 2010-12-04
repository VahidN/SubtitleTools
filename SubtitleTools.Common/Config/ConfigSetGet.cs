using System;
using System.Configuration;
using SubtitleTools.Common.MVVM;

namespace SubtitleTools.Common.Config
{
    public class ConfigSetGet
    {
        #region Methods (2)

        // Public Methods (2) 

        /// <summary>
        /// read settings from app.config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigData(string key)
        {
            //don't load on design time
            if (Designer.IsInDesignModeStatic)
                return "0";

            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = configuration.AppSettings;
            var res = appSettings.Settings[key].Value;
            if (res == null) throw new Exception(string.Format("Undefined: {0}", key));
            return res;
        }

        public static void SetConfigData(string key, string data)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = data;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        #endregion Methods
    }
}
