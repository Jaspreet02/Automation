using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BLW.Lib.Log;

namespace PreProcessing
{
    class AppConfig
    {
        /// <summary>
        /// Name of the log file.
        /// </summary>
        public static string DotDoneRootDirectory
        {
            get
            {
                return ReadValueByKeyFromConfig("DotDoneRootDirectory");
            }
        }


        /// <summary>
        /// Function Name : ReadValueByKeyFromConfig()
        /// Description   : This method reads the value of a key from App.Config file.
        /// </summary>
        /// <param name="key">Config Key</param>
        /// <returns>Value of the key</returns>
        public static string ReadValueByKeyFromConfig(string key)
        {
            string keyValue = string.Empty;
            try
            {
                keyValue = System.Configuration.ConfigurationManager.AppSettings[key];
                if (keyValue == null)
                    throw new ConfigurationErrorsException("Error : Configuration Key '" + key + "' not  found in App.Config file");
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Debug("Exception: " + ex.Message);
                throw ex;
            }
            return keyValue;
        }
    }
}
