using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{ /// <summary>
    /// R99
    /// </summary>
    [Serializable]
    public class R99
    {
        #region Ctor
        /// <summary>
        /// R99
        /// </summary>
        public R99() {/*Do nothing*/}
        /// <summary>
        /// R99
        /// </summary>
        /// <param name="recordB"></param>
        public R99(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 99 not valid - Invalid file.");
            _CashLetterCount = new RText(string.Empty, string.Empty, 3, 8, _RecordContent);
            _TotalRecordCount = new RText(string.Empty, string.Empty, 9, 16, _RecordContent);
            _TotalItemCount = new RText(string.Empty, string.Empty, 17, 24, _RecordContent);
            _FileTotalAmount = new RText(string.Empty, string.Empty, 25, 40, _RecordContent);

        }
        #endregion

        #region Private Members
        private RText _CashLetterCount;
        private RText _TotalRecordCount;
        private RText _TotalItemCount;
        private RText _FileTotalAmount;
        #endregion

        #region Properties
        public string CashLetterCount
        {
            get { return this._CashLetterCount.Value; }
        }

        public string TotalRecordCount
        {
            get { return this._TotalRecordCount.Value; }
        }

        public string TotalItemCount
        {
            get { return this._TotalItemCount.Value; }
        }

        public string FileTotalAmount
        {
            get { return this._FileTotalAmount.Value; }
        }
        #endregion
    }
}
