using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles.DDC
{
    /// <summary>
    /// Represent a DDC Data file
    /// </summary>
    public class TransactionFile
    {
        /// <summary>
        /// Load a DDC file
        /// </summary>
        /// <param name="path">path of DDC file</param>
        /// <returns></returns>
        public static Transaction Load(string path)
        {
            Transaction _Transaction = new Transaction();
            //Path Validations
            if (string.IsNullOrEmpty(path))
                throw new Exception("File path cannot be null or empty.");
            if (!System.IO.File.Exists(path))
                throw new Exception(string.Format("File not found at {0} path.", path));

            // Use using StreamReader for disposing.
            try
            {
                ASCIIEncoding strEncoding = new ASCIIEncoding();
                bool justHadByteUpdate = false;
                var readedSize = 0;
                using (var reader = new StreamReader(path))
                {
                    long fileSize = reader.BaseStream.Length;
                    int iLineNumber = 0;
                    int iCount = 0;
                    _Transaction.Name = Path.GetFileName(path);
                    // Use while != null pattern for loop
                    string line;
                    StringBuilder block = new StringBuilder();
                    while ((line = reader.ReadLine()) != null)
                    {
                        readedSize += strEncoding.GetBytes(line).Length;
                        iLineNumber++;
                        if (iLineNumber == 1)
                            continue;
                        if (line.Trim().StartsWith("010001"))
                        {
                            iCount++;
                            if (iCount == 1)
                            {
                                //this is starting of a file, no record is there to parse
                                block.AppendLine(line);
                                continue;
                            }
                            _Transaction.Records.Add(new TransactionRecord((iCount - 1), block));
                            block = new StringBuilder();
                        }
                        block.AppendLine(line);
                        if (reader.EndOfStream)
                            _Transaction.Records.Add(new TransactionRecord((iCount - 1), block));

                        //Calculate progress                                                                                               
                        if (justHadByteUpdate)
                            Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write("Reading : {0}/{1} ({2:N0}%)", (readedSize / 1024 / 1024).ToString("###,###,###") + "MB", (fileSize / 1024 / 1024).ToString("###,###,###") + "MB", readedSize / (0.01 * fileSize));
                        justHadByteUpdate = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Transaction;
        }
    }
}
