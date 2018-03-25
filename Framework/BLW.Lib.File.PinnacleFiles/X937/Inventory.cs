using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// Inventory Information
    /// </summary>
    public class Inventory
    {
        public string Name { get; set; }
        private X937 x937 = null;
        public Inventory() { }
        public Inventory(X937 _x9)
        {
            x937 = _x9;
        }
        private List<InventoryLine> _InventoryLines = new List<InventoryLine>();
        /// <summary>
        /// Lines
        /// </summary>
        public List<InventoryLine> Lines
        {
            get { return _InventoryLines; }            
        }
        /// <summary>
        /// Add Line
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(InventoryLine line)
        {
            _InventoryLines.Add(line);
        }
        public bool Write(string fileName)
        {
            FileStream fileStream;
            StreamWriter writer = null;
            string line = string.Empty;
            // Create directory, if necessary
            try
            {
                fileStream = new FileStream(fileName, FileMode.Append);
                // NOTE: Be sure you have write privileges to folder
                writer = new StreamWriter(fileStream);
                if (x937.FileHeader != null)
                    foreach (R10 cashLetter in x937.FileHeader.CashLetterHeaders)
                    {
                        foreach (R20 bundle in cashLetter.Bundles)
                        {
                            foreach (R25 check in bundle.Checks)
                            {
                                //Writing file with {Bundle Id -*- Bundle Business Date -*- Item Sequence Number -*-  BOFD Sequence Number -*- Amount  -*-  On-US -*- Aux On-US -*- Image File Name -*-  x937 File Name}
                                line = string.Format("{0} -*- {1} -*-  {2} -*-   {3} -*-  {4} -*-  {5}-*-  {6} -*-  {7} -*-  {8}", bundle.BundleID.PadRight(10, ' '),
                            bundle.BundleBusinessDate.PadRight(10, ' '), check.ECEInstitutionItemSequenceNumber.PadRight(20, ' '),
                            check.AddendumAs[0].BOFDItemSequenceNumber.PadRight(20, ' '), check.ItemAmount.PadRight(15, ' '),
                            check.OnUs.PadRight(25, ' '), check.AuxiliaryOnUs.PadRight(25, ' '), check.FrontImage.CheckFileName.PadRight(25, ' '), x937.Name.PadRight(50, ' '));
                                //Adding new line
                                this.AddLine(new InventoryLine(line));
                                writer.WriteLine(line);
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                try
                {
                    writer.Close();
                }
                catch { /* do nothing */}
            }
            return true;
        }
    } 
}
