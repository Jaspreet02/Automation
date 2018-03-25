using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace BLW.Lib.File.BLW
{   
    /// <summary>
    /// This class contains all utility functions
    /// </summary>
    public class Utility
    {      
        /// <summary>
        /// Format Date
        /// </summary>
        /// <param name="val">value to be format as date</param>
        /// <returns>return date as formated string like (dd MMM yyyy)</returns>
        public static string FormatDate(string val)
        {
            try
            {
                string newVal = string.Empty;
                if (!string.IsNullOrEmpty(val))
                {
                    var parsedDate = DateTime.ParseExact(val, Constants.EXTRACT_DATE_FORMAT, null);
                    newVal = parsedDate.ToString(Constants.DATE_FORMAT);
                }
                else
                    newVal = val;
                return newVal.Trim();
            }
            catch (Exception ex)
            {
                //throw error
                throw ex;
            }
        }
        /// <summary>
        /// Replace Prefix Zeros
        /// </summary>
        /// <param name="val">value</param>
        /// <returns>value after replace prefix zeros </returns>
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

                //In case of decimal in the begining, add a zero
                var decimalPointIndex = newVal.IndexOf(".");
                if (decimalPointIndex == 0)
                    newVal = "0" + newVal;
                return newVal.Trim();
            }
            catch (Exception ex)
            {
                //throw error
                throw ex;
            }
        }

        /// <summary>
        /// format amount  (like in US culture)
        /// </summary>
        /// <param name="val">amount as string</param>
        /// <returns>formated amount with 2 decimal places</returns>
        public static string AmountFormat(string val)
        {
            try
            {
                string newVal = string.Empty;
                if (!string.IsNullOrEmpty(val))
                {
                    if (val.Length > 1)
                    {
                        var uGBCulture = CultureInfo.CreateSpecificCulture("en-GB");
                        newVal = string.Format(uGBCulture, "{0:N2}", val);
                    }
                }
                else
                    newVal = val;
                return newVal.Trim();
            }
            catch (Exception ex)
            {
                //throw error
                throw ex;
            }

        }
    }
    /// <summary>
    /// Format Types
    /// </summary>
    public enum FormatTypes
    {
        /// <summary>
        /// Date format
        /// </summary>
        DateFormat,
        /// <summary>
        /// Remove prefix zero
        /// </summary>
        RemovePrefixZeros,
        /// <summary>
        /// No opertion 
        /// </summary>
        None
    }

}
