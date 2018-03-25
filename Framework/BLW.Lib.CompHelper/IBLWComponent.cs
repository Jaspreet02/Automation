using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.CoreUtility;

namespace BLW.Lib.CompHelper
{
    /**************** BLW.Lib.CompHelper [Information] **********************
 * Written By : Ravi
 * Date : 24-09-2014
 * Description : It will be the base of any component runs in automation
 ***************************************************************************/
    /// <summary>
    /// Represent the base of any component runs in automation
    /// </summary>
    public interface IBLWComponent
    {
        /// <summary>
        /// Initialize Components default behaviour, Logging, trigger file validation
        /// </summary>
        /// <param name="args"></param>
        bool Initialize(string[] args);
        /// <summary>
        /// Process trigger file
        /// </summary>
        /// <returns></returns>
        TriggerFileReader ProcessTriggerFile();
        /// <summary>
        /// Get mapper initailze with new if not exist, otherwise return existing
        /// </summary>
        /// <returns></returns>
        PatternMatchingMapper GetMapper();     
        /// <summary>
        /// Completed successfully
        /// </summary>
        bool Completed();
        /// <summary>
        /// Completed with an error
        /// </summary>
        bool Error(Exception ex);
        /// <summary>
        /// Delete system generated guid entry from sqlite database
        /// </summary>        
        bool DeleteDbEntry();
    }
}
