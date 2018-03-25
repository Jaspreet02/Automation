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
    /// x937
    /// </summary>
    [Serializable]
    public class X937
    {

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public X937()
        {
            Loaded = false;
        }
        #endregion

        string filePath = null;

        /// <summary>
        /// file Path
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        /// <summary>
        /// File Name
        /// </summary>
        public string Name
        {
            get
            {
                string name = null;
                if (!string.IsNullOrEmpty(filePath))
                    name = Path.GetFileName(filePath);
                return name;
            }
        }
        /// <summary>
        /// Is file loading complete
        /// </summary>
        public bool Loaded { get; set; }
        /// <summary>
        /// x937 file header
        /// </summary>
        public R01 FileHeader { get; set; }
        /// <summary>
        /// file Control        
        /// </summary>
        public R99 FileControl { get; set; }
        /// <summary>
        /// Split x937 file to specified path
        /// <para>Also generate inventory file</para>
        /// </summary>
        /// <param name="toPath">path to which split file</param>
        /// <param name="inv">output Inventory (metadata) file</param>
        /// <param name="frontOnly">if true then front image only, else both images</param>
        /// <returns>return true if splitted successfully</returns>
        public bool Split(string toPath, out  Inventory inv, bool frontOnly = true)
        {
            inv = new Inventory();
            int checksCount = 0;
            bool justHadByteUpdate = false;
            List<string> fileLines = new List<string>();
            //Splits images in parallel Processing     
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                long fileSize = fs.Length;
                long readedSize = 0;
                using (var reader = new BinaryReader(fs, System.Text.Encoding.GetEncoding(37)))
                {
                    if (FileHeader != null)
                    {
                        foreach (R10 r10 in FileHeader.CashLetterHeaders)
                        {
                            foreach (R20 r20 in r10.Bundles)
                            {
                                foreach (var check in r20.Checks)
                                {
                                    try
                                    {
                                        //Add a line to inventory file
                                        string checkFrontImageName = string.Format(Path.Combine(toPath, "{0}_Front.TIFF"), checksCount.ToString());

                                        string line = string.Format("{0} -*- {1} -*-  {2} -*-   {3} -*-  {4} -*-  {5} -*- {6} -*- {7} -*- {8}",
                                            r20.BundleID.PadRight(10, ' '), r20.BundleBusinessDate.PadRight(10, ' '), check.ECEInstitutionItemSequenceNumber.PadRight(20, ' '),
                                      check.AddendumAs[0].BOFDItemSequenceNumber.PadRight(20, ' '), check.ItemAmount.PadRight(15, ' '), check.OnUs.PadRight(25, ' '),
                                      check.AuxiliaryOnUs.PadRight(25, ' '), string.Format("{0}", checkFrontImageName).PadRight(25, ' '), this.Name.PadRight(50, ' '));

                                        //Adding new line
                                        inv.AddLine(new InventoryLine(line));
                                        fileLines.Add(line);

                                        //Save front image                                        
                                        check.FrontImage.CheckFileName = checkFrontImageName;
                                        reader.BaseStream.Position = check.FrontImage.ImageStartIndex;
                                        byte[] outFrontArr = reader.ReadBytes(check.FrontImage.ImageLength);
                                        System.IO.File.WriteAllBytes(checkFrontImageName, outFrontArr);

                                        //Save back image            
                                        if (!frontOnly)
                                        {
                                            string checkBackImageName = string.Format(Path.Combine(toPath, "{0}_Back.TIFF"), checksCount.ToString());
                                            check.BackImage.CheckFileName = checkBackImageName;
                                            reader.BaseStream.Position = check.BackImage.ImageStartIndex;
                                            byte[] outBackArr = reader.ReadBytes(check.BackImage.ImageLength);
                                           System.IO. File.WriteAllBytes(checkBackImageName, outBackArr);
                                        }
                                        readedSize = reader.BaseStream.Position;
                                        checksCount++;
                                        if (checksCount % 250 == 0)
                                        {
                                            if (justHadByteUpdate)
                                                Console.SetCursorPosition(0, Console.CursorTop);
                                            Console.Write("Splitting Check Images :  {0}/{1} ({2:N0}%)", (readedSize / 1024 / 1024).ToString("###,###,###") + "MB", (fileSize / 1024 / 1024).ToString("###,###,###") + "MB", readedSize / (0.01 * fileSize));
                                            justHadByteUpdate = true;
                                            Thread.Sleep(1);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Error in x937 file extraction.", ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("checks splitted successfully.");
           System.IO.File.WriteAllLines(string.Format(Path.Combine(toPath, "{0}.csv"), this.Name), fileLines.ToArray());
            Console.WriteLine("inventory file written successfully.");
            return true;
        }
    }    
}
