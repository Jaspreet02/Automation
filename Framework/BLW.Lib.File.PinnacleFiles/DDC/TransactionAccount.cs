using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles.DDC
{
    public class TransactionAccount
    {
        #region Constructor
        /// <summary>
        /// Represent Transaction Account
        /// </summary>
        /// <param name="srNo"></param>
        /// <param name="RawData"></param>
        public TransactionAccount(int srNo, StringBuilder RawData)
        {
            Checks = new List<CheckRecord>();
            Parse(srNo, RawData.ToString());
        }
        #endregion

        #region Methods
        private bool Parse(int srNo, string block)
        {
            try
            {
                using (StringReader reader = new StringReader(block))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string checkNo = string.Empty;
                        string sequenceNo = string.Empty;
                        string description = string.Empty;
                        string amount = string.Empty;
                        string date = string.Empty;
                        if (line.StartsWith("011100"))
                        {
                            sequenceNo = Util.ReplacePrefixZeros(line.ToString().Substring(49, 18));
                            checkNo = Util.ReplacePrefixZeros(line.ToString().Substring(95, 10));
                            description = Util.ReplacePrefixZeros(line.ToString().Substring(119, 80));
                            amount = Util.ReplacePrefixZeros(line.ToString().Substring(35, 14));
                            date = line.ToString().Substring(67, 8);
                        }

                        //Handling Check only
                        if (description.ToUpper().Equals("CHECK"))
                            description = "CHECK ";
                        //Either check or deposit
                        //Adding space with "Check ", to avoid checkers and other descriptions that are not checks
                        if (description.ToUpper().StartsWith("DEPOSIT") || description.ToUpper().StartsWith("CHECK ") || description.ToUpper().StartsWith("CLOSING WITHDRAWAL"))
                            this.Checks.Add(new CheckRecord(sequenceNo, checkNo, amount, description, date));
                    }
                }
                return true;
            }
            catch (Exception ex) { throw new Exception("invalid file"); }
        }
        #endregion

        #region Entities
        public string AccountNumber { get; set; }
        public List<CheckRecord> Checks { get; set; }



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
