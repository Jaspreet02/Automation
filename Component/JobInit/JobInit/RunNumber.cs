using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using System.Configuration;

using System.IO;


namespace JobInit
{
    public class RunNumber
    {

        public string GetRunNumber(string clientCode, string appCode, string lastRunnumber)
        {
            char month = Convert.ToChar(64 + DateTime.Now.Month);
            char year = Convert.ToChar(DateTime.Now.Year - 1936);
            int iDay =   DateTime.Now.Day;
            if (iDay > 26)
                iDay -= 26;

            char day = Convert.ToChar(64 + iDay);          
            var datePart = year.ToString() + month.ToString() + day.ToString();
            string lastUniqueNumber;
            if (lastRunnumber != "000000")
            {
                string lastRunNumber = lastRunnumber;//from service
                string[] runNumberArr = lastRunNumber.Split('_');
                lastUniqueNumber = runNumberArr[3];
            }
            else
            {
                lastUniqueNumber = "000000";
            }
            return datePart + "_" + clientCode + "_" + appCode + "_" + GetNextUniqueNumber(lastUniqueNumber);
        }


        /// <summary>
        /// Function    : GetNextUniqueNumber
        /// Description : This function gentare combination of unique id.
        ///               Unique Id start from 000001 to ZZZZZZ
        ///               Exmple : 
        ///                     1. 00001 to 0000Z
        ///                     2. 00011 to 000ZZ
        /// </summary>
        /// <param name="uniqueNumber">Last unique number</param>
        /// <param name="sequentialLength">Maximum length of unique number</param>
        /// <returns>Sequential number</returns>
        public string GetNextUniqueNumber(string uniqueNumber, int sequentialLength = 6)
        {
            if (string.IsNullOrEmpty(uniqueNumber))
                throw new ArgumentNullException();

            if (uniqueNumber.Length > sequentialLength)
                uniqueNumber = uniqueNumber.Substring((uniqueNumber.Length - sequentialLength), sequentialLength);


            uniqueNumber = uniqueNumber.PadLeft(sequentialLength, '0');


            var originalStack = new Stack();
            var increamentStack = new Stack();

            var valueArr = uniqueNumber.ToCharArray();

            /*****
             *  Enter All Characters of unique key into Original Stack.
             ****/
            foreach (char value in valueArr)
            {
                originalStack.Push(value);
            }

            /***  Start Increamenting Process   ***/
            bool incrementFlag = false;
            for (int i = 0; i < sequentialLength; i++)
            {
                var currentDigit = originalStack.Pop();

                if (incrementFlag == false)
                {

                    switch (Convert.ToInt32(currentDigit))
                    {
                        case 57:
                            {
                                increamentStack.Push('A');
                                incrementFlag = true;
                                break;
                            }
                        case 90:
                            {
                                increamentStack.Push('0');
                                break;
                            }
                        default:
                            {
                                if (Convert.ToInt32(currentDigit) < 48)
                                    throw new ArgumentNullException("uniqueNumber is not  valid. ");

                                if ((Convert.ToInt32(currentDigit) > 57) && (Convert.ToInt32(currentDigit) < 65))
                                    throw new ArgumentNullException("uniqueNumber is not  valid. ");

                                if (Convert.ToInt32(currentDigit) > 99)
                                    throw new ArgumentNullException("uniqueNumber is not  valid. ");

                                var incrementalValue = Convert.ToInt32(currentDigit) + 1;
                                increamentStack.Push(Convert.ToChar(incrementalValue));
                                incrementFlag = true;
                                break;
                            }
                    }
                }
                else
                {
                    increamentStack.Push(currentDigit);
                }

            }

            var incrementalId = "";
            for (int i = 0; i < sequentialLength; i++)
            {
                incrementalId += Convert.ToString(increamentStack.Pop());
            }
            return incrementalId;
        }


    }
}
