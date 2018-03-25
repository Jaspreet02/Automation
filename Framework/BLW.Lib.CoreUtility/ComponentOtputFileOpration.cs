using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BLW.Lib.CoreUtility
{
    public class ComponentOtputFileOpration : FileOperation
    {
        /// <summary>
        /// Method: MoveAllFiles
        /// description: Move or Copy All files from input location to outputlocation based on file mask
        /// </summary>
        /// <param name="inputLocation"></param>
        /// <param name="outputLocation"></param>
        /// <param name="mask">Filemask</param>
        /// <param name="isCopy">true for Copy opration,false(byDefault) for Move opration</param>

        public void MoveAllFiles(string inputLocation, string outputLocation, string mask = "*.*", bool isCopy = false)
        {
            if (!Directory.Exists(inputLocation))
                throw new Exception("Input Location does not exists. ");

            //Get all files from output location 
            var files = Directory.GetFiles(inputLocation, mask);

            if (files.Count() > 0)
            {
                #region Move All files to output location

                foreach (var file in files)
                {
                    try
                    {
                        MoveFile(file, outputLocation, isCopy);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                #endregion

            }
            else
                Console.WriteLine("No files exists on input location " + inputLocation);



        }

    }
}
