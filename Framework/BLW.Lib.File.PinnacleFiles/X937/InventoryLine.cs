using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// Represent a Inventory line
    /// </summary>
    public class InventoryLine
    {
        #region Constructor
        /// <summary>
        /// Saperate a line on the basis of default saperators (-*-)
        /// </summary>
        /// <param name="line">line</param>      
        public InventoryLine(string line)
        {
            Split(line);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Split the line using 
        /// </summary>
        /// <param name="line">line</param>
        private void Split(string line)
        {
            try
            {
                _Line = line;
                string[] splitItems = line.Split(_Saperator, StringSplitOptions.None);
                if (splitItems.Count() == 9)
                {                    
                    BundleId = splitItems[0].Trim();
                    BundleBusinessDate = splitItems[1].Trim();
                    ECESequenceNumber = Util.ReplacePrefixZeros(splitItems[2].Trim());
                    BOFDSequenceNumber = Util.ReplacePrefixZeros(splitItems[3].Trim());
                    Amount = Util.ReplacePrefixZeros(splitItems[4].Trim());
                    OnUS = splitItems[5].Trim();
                    AuxOnUS = splitItems[6].Trim();
                    ImageFile = splitItems[7].Trim();
                    File = splitItems[8].Trim();
                }
                else
                    throw new Exception("Unreadable format of .Inventory file");
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region Public Member
        #region Entities
        public string BundleId { get; set; }
        public string BundleBusinessDate { get; set; }
        public string ECESequenceNumber { get; set; }
        public string BOFDSequenceNumber { get; set; }
        public string Amount { get; set; }
        public string OnUS { get; set; }
        public string AuxOnUS { get; set; }
        public string ImageFile { get; set; }
        public string File { get; set; }
        /// <summary>     
        /// <para>Retrive check number from OnUS or Aux OnUS</para>
        /// <para>From OnUS field if then are 3 parts after split with "/", otherwise Aux OnUS field with be check number</para>    
        /// </summary>
        public string CheckNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(OnUS))
                {
                    var OnUsParts = OnUS.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    switch (OnUsParts.Count())
                    {
                        case 2:
                            checkNumber = AuxOnUS;
                            break;
                        case 3:
                            checkNumber = OnUsParts[0];
                            break;
                        default:
                            break;
                    }
                }
                return Util.ReplacePrefixZeros(checkNumber);
            }
        }
        public string AccountNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(OnUS))
                {
                    var OnUsParts = OnUS.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    switch (OnUsParts.Count())
                    {
                        case 2:
                            accountNumber = OnUsParts[0];
                            break;
                        case 3:
                            accountNumber = OnUsParts[1];
                            break;
                        default:
                            break;
                    }
                }
                return Util.ReplacePrefixZeros(accountNumber);
            }
        }
        #endregion

        #region Private Variables
        private string _Line = string.Empty;
        private string[] _Saperator = { " -*- " };
        string checkNumber = string.Empty;
        string accountNumber = string.Empty; 
        #endregion
        #endregion
    }
}
