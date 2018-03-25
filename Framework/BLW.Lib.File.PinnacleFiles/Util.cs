using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles
{
    /// <summary>
    /// Utility class contains common methods
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Replace prefix Zeros
        /// </summary>
        /// <param name="val">value</param>
        /// <returns></returns>
        public static string ReplacePrefixZeros(string val)
        {
            try
            {
                string newVal = string.Empty;
                bool prefixZero = true;
                if (!string.IsNullOrEmpty(val))
                {
                    for (int i = 0; i < val.Length; i++)
                    {
                        if (prefixZero)
                        {
                            if (val[i].ToString().Trim().Equals("0"))
                                continue;
                            else
                                prefixZero = false;
                        }
                        newVal += val[i];
                    }
                }
                else
                    newVal = val;
                return newVal.Trim();
            }
            catch (Exception)
            {
                return val;
            }
        }
    }
}
