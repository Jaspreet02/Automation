using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles.DDC
{
    /// <summary>
    /// Check Record
    /// </summary>
    public class CheckRecord
    {
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceNo"></param>
        /// <param name="checkNo"></param>
        /// <param name="amount"></param>
        /// <param name="description"></param>
        /// <param name="date"></param>
        public CheckRecord(string sequenceNo, string checkNo, string amount, string description, string date)
        {
            this.CheckNumber = checkNo;
            this.Description = description;
            this.Amount = amount;
            this.SequenceNumber = sequenceNo;
            this.Date = date;
        }
        #endregion

        #region Entities
        public string CheckNumber { get; set; }
        public string SequenceNumber { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        #endregion
    }   
}
