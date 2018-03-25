using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{  
    /// <summary>
    /// R70
    /// </summary>
    [Serializable]
    public class R70
    {
        #region Ctor
        public R70() {/*Do nothing*/}
        public R70(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 70 not valid - Invalid file.");
            _ItemsWithinBundleCount = new RText(string.Empty, string.Empty, 3, 6, _RecordContent);
            _BundleTotalAmount = new RText(string.Empty, string.Empty, 7, 18, _RecordContent);
            _ImageswithinBundleCount = new RText(string.Empty, string.Empty, 31, 35, _RecordContent);
        }
        #endregion

        #region Private Members
        private RText _ItemsWithinBundleCount;
        private RText _BundleTotalAmount;
        private RText _ImageswithinBundleCount;

        #endregion

        #region Properties

        public string ItemsWithinBundleCount
        {
            get { return this._ItemsWithinBundleCount.Value; }
        }

        public string BundleTotalAmount
        {
            get { return this._BundleTotalAmount.Value; }
        }

        public string ImageswithinBundleCount
        {
            get { return this._ImageswithinBundleCount.Value; }
        }

        #endregion
    }   
}
