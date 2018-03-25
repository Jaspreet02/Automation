using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{
   /// <summary>
    /// R90
    /// </summary>
    [Serializable]
    public class R90
    {
        #region Ctor
        public R90() {/*Do nothing*/ }
        public R90(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 90 not valid - Invalid file.");
            _BundleCount = new RText(string.Empty, string.Empty, 3, 8, _RecordContent);
            _ItemsWithinCashletterCount = new RText(string.Empty, string.Empty, 9, 16, _RecordContent);
            _CashLetterTotalAmount = new RText(string.Empty, string.Empty, 17, 30, _RecordContent);
            _ImageswithinCashletterCount = new RText(string.Empty, string.Empty, 31, 39, _RecordContent);
        }
        #endregion

        #region Private Members
        private RText _BundleCount;
        private RText _ItemsWithinCashletterCount;
        private RText _CashLetterTotalAmount;
        private RText _ImageswithinCashletterCount;

        #endregion

        #region Properties
        public string BundleCount
        {
            get { return this._BundleCount.Value; }
        }

        public string ItemsWithinCashletterCount
        {
            get { return this._ItemsWithinCashletterCount.Value; }
        }

        public string CashLetterTotalAmount
        {
            get { return this._CashLetterTotalAmount.Value; }
        }
        public string ImageswithinCashletterCount
        {
            get { return this._ImageswithinCashletterCount.Value; }
        }
        #endregion
    }   
}
