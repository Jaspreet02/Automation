using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// Represent x9.37 file
    /// </summary>
    [Serializable]
    public class X937File
    {
        /// <summary>
        /// Load file
        /// </summary>
        /// <param name="path">full path</param>
        /// <returns></returns>
        public static X937 Load(string path)
        {
            X937 _X937 = new X937();
            bool justHadByteUpdate = false;
            // Use using StreamReader for disposing.
            #region Read x937
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                _X937.FilePath = path;
                long fileSize = fs.Length;
                using (var reader = new BinaryReader(fs, System.Text.Encoding.GetEncoding(37)))
                {
                    var curPos = 0;
                    var imgNbr = 0;
                    //Read length of record
                    byte[] reclenB = reader.ReadBytes(4);
                    //reverse array
                    Array.Reverse(reclenB);
                    //Convert to number
                    int reclen = BitConverter.ToInt32(reclenB, 0);
                    var readedSize = 0;
                    byte[] recB = new byte[] { };
                    var counter = 0;

                    //defining variables                      
                    // counts
                    int fileRecCount = 0;
                    int checkCount = 0;
                    // Flags for start and end of sections              
                    bool fileStarted = false;   //Flag for header
                    bool fileEnded = false;    // flag for footer
                    bool clStarted = false;   // flag for cash letter header
                    bool clEnded = false;     //flag for cash letter footer
                    bool bundleStarted = false; // flag for bundle header
                    bool bundleEnded = false;   // flage for bundle footer
                    bool checkStarted = false;
                    bool checkFront50 = false;
                    bool checkFront52 = false;
                    bool checkBack50 = false;
                    bool checkBack52 = false;
                    R01 fileHeader = null;
                    R99 fileControl = null;
                    R10 cashLetterHeader = null;
                    R20 bundleHeader = null;
                    R25 checkDetail = null;
                    //Read complete file
                    do
                    {
                        recB = reader.ReadBytes(reclen);
                        readedSize += reclen + 4;
                        curPos += reclen;
                        fileRecCount += 1;

                        //getting values                               
                        var rec = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recB));
                        var recordType = (RecordTypes)Enum.Parse(typeof(RecordTypes), rec.Substring(0, 2), true);

                        switch (recordType)
                        {
                            #region Record Type 01
                            case RecordTypes.FileHeader:
                                fileStarted = true;
                                fileHeader = new R01(recB);
                                break;
                            #endregion

                            #region Record Type 10
                            case RecordTypes.CashLetterHeader:
                                if (fileStarted)
                                {
                                    if (clStarted)
                                    {
                                        if (clEnded)
                                            clEnded = false;
                                        else
                                            throw new Exception("No Cash Letter control record - Invalid file.");
                                    }
                                    else
                                        clStarted = true;
                                    cashLetterHeader = new R10(recB);
                                }
                                else
                                    throw new Exception("No File Header Record - Invalid file.");
                                break;
                            #endregion

                            #region Record Type 20
                            case RecordTypes.BundleHeader:
                                if (fileStarted)
                                {
                                    if (clStarted)
                                    {
                                        if (bundleStarted)
                                        {
                                            if (bundleEnded)
                                            {
                                                bundleEnded = false;
                                            }
                                            else
                                                throw new Exception("No Bundle Control record - Invalid file.");
                                        }
                                        else
                                        {
                                            bundleStarted = true;
                                        }
                                    }
                                    else
                                        throw new Exception("No Cash Letter Header record - Invalid file.");
                                }
                                else
                                {
                                    // no file header yet
                                    throw new Exception("No File Header Record - Invalid file.");
                                }
                                bundleHeader = new R20(recB);
                                break;
                            #endregion

                            #region Record Type 25
                            case RecordTypes.CheckDetail:
                                if (bundleStarted && !bundleEnded)
                                {
                                    if (checkStarted)
                                    {
                                        //make sure we got everything for previous check
                                        if (!checkFront50)
                                        {
                                            //no check front 50
                                            throw new Exception("No Check Image Detail Record 50 : Front - Invalid file.");
                                        }
                                        if (!checkFront52)
                                        {
                                            // no check front 52
                                            throw new Exception("No Check Image Data Record 52 : Front - Invalid file.");
                                        }
                                        if (!checkBack50)
                                        {
                                            //no check back 50
                                            throw new Exception("No Check Image Detail Record 50 : Back - Invalid file.");
                                        }
                                        if (!checkBack52)
                                        {
                                            //no check back 52
                                            throw new Exception("No Check Image Data Record 52 : Back - Invalid file.");
                                        }
                                        checkBack50 = false;
                                        checkBack52 = false;
                                        checkFront50 = false;
                                        checkFront52 = false;
                                    }
                                    else
                                        checkStarted = true;
                                }
                                else
                                    // no bundle header yet
                                    throw new Exception("No Bundle Header Record - Invalid file.");
                                //Add previous check
                                if (checkDetail != null)
                                    bundleHeader.Checks.Add(checkDetail);
                                checkCount += 1;
                                checkDetail = new R25(recB);

                                break;
                            #endregion

                            #region Record Type 26
                            case RecordTypes.CheckDetailAddendumA:
                                checkDetail.AddendumAs.Add(new R26(recB));
                                break;
                            #endregion

                            #region Record Type 28
                            case RecordTypes.CheckDetailAddendumC:
                                checkDetail.AddendumCs.Add(new R28(recB));
                                break;
                            #endregion

                            #region Record Type 50
                            case RecordTypes.ImageViewDetail:
                                if (checkStarted)
                                {
                                    if (checkFront50)
                                    {
                                        //back of check
                                        checkBack50 = true;
                                        checkDetail.BackImageDetail = new R50(recB);
                                    }
                                    else
                                    {
                                        //front of check
                                        checkFront50 = true;
                                        checkDetail.FrontImageDetail = new R50(recB);
                                    }
                                }
                                break;
                            #endregion

                            #region Record Type 52
                            case RecordTypes.ImageViewData:
                                if (checkStarted)
                                    if (checkFront52)
                                    {
                                        //back image of check
                                        checkBack52 = true;
                                        checkDetail.BackImage = new R52(recB, curPos, false);
                                    }
                                    else
                                    {
                                        //front image of check
                                        checkFront52 = true;
                                        checkDetail.FrontImage = new R52(recB, curPos, true);
                                    }
                                imgNbr += 1;
                                break;
                            #endregion

                            #region Record Type 70
                            case RecordTypes.BundleControl:
                                bundleEnded = true;
                                cashLetterHeader.Bundles.Add(bundleHeader);
                                break;
                            #endregion

                            #region Record Type 90
                            case RecordTypes.CashLetterControl:
                                clEnded = true;
                                fileHeader.CashLetterHeaders.Add(cashLetterHeader);
                                break;
                            #endregion

                            #region Record Type 99
                            case RecordTypes.FileControl:
                                fileEnded = true;
                                fileControl = new R99(recB);
                                _X937.FileHeader = fileHeader;
                                _X937.FileControl = fileControl;
                                break;
                            #endregion

                            default:
                                //throw new Exception("Unexpected record type occured.");
                                break;
                        }
                        reclenB = reader.ReadBytes(4);
                        curPos += 4;
                        if (reclenB.Length == 4)
                        {
                            Array.Reverse(reclenB);
                            reclen = BitConverter.ToInt32(reclenB, 0);
                        }
                        else
                            reclen = 0;
                        //Showing process
                        if (counter % 400 == 0)
                        {
                            //Calculate progress                                                                                               
                            if (justHadByteUpdate)
                                Console.SetCursorPosition(0, Console.CursorTop);
                            Console.Write("Reading : {0}/{1} ({2:N0}%)", (readedSize / 1024 / 1024).ToString("###,###,###") + "MB", (fileSize / 1024 / 1024).ToString("###,###,###") + "MB", readedSize / (0.01 * fileSize));
                            justHadByteUpdate = true;
                            Thread.Sleep(1);
                        }
                        counter++;
                    } while (reclen > 0);

                }
            }
            #endregion
            Console.Write(Environment.NewLine);
            _X937.Loaded = true;
            return _X937;
        }
    }
}
