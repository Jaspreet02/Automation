using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{   
    /// <summary>
    /// Record 50
    /// </summary>
    [Serializable]
    public class R50
    {
        #region Ctor
        public R50()
        {
        }
        public R50(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 50 not valid - Invalid file.");
            _ImageIndicator = new RText(string.Empty, string.Empty, 3, 3, string.Empty);
            _ImageCreatorRoutingNumber = new RText(string.Empty, string.Empty, 4, 12, string.Empty);
            _ImageCreatorDate = new RText(string.Empty, string.Empty, 13, 20, string.Empty);
            _ImageViewFormatIndicator = new RText(string.Empty, string.Empty, 21, 22, string.Empty);
            _ImageViewCompressionAlgorithmIdentifier = new RText(string.Empty, string.Empty, 23, 24, string.Empty);
            _ImageViewDataSize = new RText(string.Empty, string.Empty, 25, 31, string.Empty);
            _ViewSideIndicator = new RText(string.Empty, string.Empty, 32, 32, string.Empty);
            _ViewDescriptor = new RText(string.Empty, string.Empty, 33, 34, string.Empty);
            _ImageTIFFVarianceIndicator = new RText(string.Empty, string.Empty, 66, 66, string.Empty);
            _OverrideIndicator = new RText(string.Empty, string.Empty, 67, 67, string.Empty);
            _Reserved = new RText(string.Empty, string.Empty, 68, 80, string.Empty);
        }
        #endregion

        #region Private Members
        private RText _ImageIndicator;
        private RText _ImageCreatorRoutingNumber;
        private RText _ImageCreatorDate;
        private RText _ImageViewFormatIndicator;
        private RText _ImageViewCompressionAlgorithmIdentifier;
        private RText _ImageViewDataSize;
        private RText _ViewSideIndicator;
        private RText _ViewDescriptor;
        private RText _ImageTIFFVarianceIndicator;
        private RText _OverrideIndicator;
        private RText _Reserved;
        #endregion

        #region Properties
        internal string ImageIndicator
        {
            get { return this._ImageIndicator.Value; }
        }
        public string ImageCreatorRoutingNumber
        {
            get { return this._ImageCreatorRoutingNumber.Value; }
        }

        public string ImageCreatorDate
        {
            get { return this._ImageCreatorDate.Value; }
        }

        public string ImageViewFormatIndicator
        {
            get { return this._ImageViewFormatIndicator.Value; }
        }

        public string ImageViewCompressionAlgorithmIdentifier
        {
            get { return this._ImageViewCompressionAlgorithmIdentifier.Value; }
        }

        public string ImageViewDataSize
        {
            get { return this._ImageViewDataSize.Value; }
        }

        public string ViewSideIndicator
        {
            get { return this._ViewSideIndicator.Value; }
        }

        public string ViewDescriptor
        {
            get { return this._ViewDescriptor.Value; }
        }

        public string ImageTIFFVarianceIndicator
        {
            get { return this._ImageTIFFVarianceIndicator.Value; }
        }

        public string OverrideIndicator
        {
            get { return this._OverrideIndicator.Value; }
        }

        public string Reserved
        {
            get { return this._Reserved.Value; }
        }
        #endregion
    } 
}
