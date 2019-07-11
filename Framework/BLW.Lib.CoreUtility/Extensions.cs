using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BLW.Lib.CoreUtility
{
    public static class Extensions
    {
        /// <summary>
        /// Method: IsFileLocked
        /// Description: this is used to check whether the file is in use or not.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(this FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                return true;
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        
       

    }
}
