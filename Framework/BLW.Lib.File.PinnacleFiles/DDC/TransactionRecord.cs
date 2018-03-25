using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles.DDC
{   /// <summary>
    /// Represent a Transaction record
    /// </summary>
    public class TransactionRecord
    {
        #region Constructor
        /// <summary>
        /// Represent Transaction record
        /// </summary>
        /// <param name="srNo"></param>
        /// <param name="RawData"></param>
        public TransactionRecord(int srNo, StringBuilder RawData)
        {
            SubAccounts = new List<TransactionAccount>();
            Parse(srNo, RawData.ToString(), Saperator);
        }
        #endregion

        #region Methods
        private bool Parse(int srNo, string block, string[] saperator)
        {
            try
            {
                int iCount = 0;
                StringBuilder newSubBlock = new StringBuilder();
                TransactionAccount subAccount;
                string subAccountNumber = string.Empty;
                using (StringReader reader = new StringReader(block))
                {
                    string line;
                    bool subAccountStarted = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        //Considering "010100" sub account as main account
                        if (line.StartsWith("010001"))
                        {
                            AccountNumber = Util.ReplacePrefixZeros(line.ToString().Substring(33, 17));
                        }
                        if (line.StartsWith("010100"))
                        {
                            subAccountStarted = true;
                            iCount++;
                            if (iCount == 1)
                                subAccountNumber = Util.ReplacePrefixZeros(line.ToString().Substring(33, 17));
                            else
                            {
                                subAccount = new TransactionAccount(iCount, newSubBlock);
                                subAccount.AccountNumber = subAccountNumber;
                                SubAccounts.Add(subAccount);
                                subAccountNumber = Util.ReplacePrefixZeros(line.ToString().Substring(33, 17));
                                newSubBlock = new StringBuilder();
                            }
                        }
                        if (subAccountStarted)
                        {
                            newSubBlock.AppendLine(line);
                        }
                    }
                }
                subAccount = new TransactionAccount(iCount, newSubBlock);
                subAccount.AccountNumber = subAccountNumber;
                SubAccounts.Add(subAccount);
                return true;
            }
            catch (Exception ex) { return false; }
        }
        #endregion


        #region Entities
        public string AccountNumber { get; set; }
        public List<TransactionAccount> SubAccounts { get; set; }



        private string[] _Saperator = { "|" };
        public string Line;
        /// <summary>
        /// 
        /// </summary>
        public string[] Saperator
        {
            get { return _Saperator; }
            set { _Saperator = value; }
        }
        #endregion
    }
}
