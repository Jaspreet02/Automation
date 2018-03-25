using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.PinnacleFiles.DDC
{  
    /// <summary>
    /// Transaction Information
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public Transaction() { }
        /// <summary>
        /// Represent transaction records
        /// </summary>
        private List<TransactionRecord> _Records = new List<TransactionRecord>();
        /// <summary>
        /// Records
        /// </summary>
        public List<TransactionRecord> Records
        {
            get { return _Records; }
        }
        /// <summary>
        /// Add a  new line
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(TransactionRecord line)
        {
            if (line == null)
                throw new ArgumentNullException("Invalid Transacton line added");
            _Records.Add(line);
        }
        /// <summary>
        /// Name of DDC file
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// No of Transaction/Statement in DDC file
        /// </summary>
        public int Count
        {
            get
            {
                return _Records.Count();
            }
        }
    }  
}
