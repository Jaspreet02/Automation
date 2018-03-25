using System;
using System.Configuration;

namespace BLW.Modules.Helpers.ZipExtractor
{
    /// <summary>
    /// This class defines a read only property corresponding to each key defined in the configuration file
    /// </summary>
    public static class AppConfig
    {

        /// <summary>
        /// Input Path
        /// </summary>
        public static bool FileOperation { get { return GetValueByKey("FileOperation").ToUpper().Trim().Equals("MOVE") ? true : false; } }
        /// <summary>
        /// Gets the value of a key from app.config file throws error otherwise.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetValueByKey(String key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            else
            {
                throw new System.Configuration.ConfigurationErrorsException("Error : " + key + " key not found in  AppConfig file of current Application");
            }
        }
    }
}
