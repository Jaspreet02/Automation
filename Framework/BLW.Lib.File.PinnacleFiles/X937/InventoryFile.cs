using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// This is the metadata file from x937 files
    /// </summary>
    public class InventoryFile
    {
        /// <summary>
        /// Load a file from specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Inventory Load(string path)
        {
            //Path Validations
            if (string.IsNullOrEmpty(path))
                throw new Exception("File path cannot be null or empty.");
            if (!System.IO.File.Exists(path))
                throw new Exception(string.Format("File not found at {0} path.", path));
            Inventory _Inventory = new Inventory();
            _Inventory.Name = Path.GetFileName(path);
            // Use using StreamReader for disposing.
            using (var reader = new StreamReader(path))
            {
                int iCount = 0;                
                // Use while != null pattern for loop
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    iCount++;
                    //Skipping header
                    if (iCount == 1)
                        continue;
                    //Add lines to metadata file
                    _Inventory.Lines.Add(new InventoryLine(line));
                }
            }
            return _Inventory;
        }
    }
}
