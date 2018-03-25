using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.SqliteHelper
{
    /// <summary>
    /// Licence types
    /// </summary>   
    public enum LicenceType
    {
        NONE,
        /// <summary>
        /// Trigger Based
        /// </summary>
        TRIGGERBASE,
        /// <summary>
        /// Time Interval Based
        /// </summary>
        PERIODIC
    }
    /// <summary>
    /// 
    /// </summary>
    public class Constants
    {
        public const int UNLIMITED_INSTANCES = -1;
    }
}
