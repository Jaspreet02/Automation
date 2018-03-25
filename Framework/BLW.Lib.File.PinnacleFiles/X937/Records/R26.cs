using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{
  /// <summary>
    /// Record 26
    /// </summary>
    [Serializable]
    public class R26
    {
        #region Ctor
        public R26()
        {
        }
        public R26(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            _RecordNumber = new RText(string.Empty, string.Empty, 3, 3, _RecordContent);
            _BOFDRoutingNumber = new RText(string.Empty, string.Empty, 4, 12, _RecordContent);
            _BOFDBusinessEndorsementDate = new RText(string.Empty, string.Empty, 13, 20, _RecordContent);
            _BOFDItemSequenceNumber = new RText(string.Empty, string.Empty, 21, 35, _RecordContent);
            _DepositAccountNumberatBOFD = new RText(string.Empty, string.Empty, 36, 73, _RecordContent);
            _BOFDDepositBranch = new RText(string.Empty, string.Empty, 36, 73, _RecordContent);
            _PayeeName = new RText(string.Empty, string.Empty, 36, 73, _RecordContent);
            _TruncationIndicator = new RText(string.Empty, string.Empty, 74, 74, _RecordContent);
            _BOFDConversionIndicator = new RText(string.Empty, string.Empty, 75, 78, _RecordContent);
            _BOFDCorrectionIndicator = new RText(string.Empty, string.Empty, 75, 78, _RecordContent);
            _UserField = new RText(string.Empty, string.Empty, 79, 79, _RecordContent);
            _Reserved = new RText(string.Empty, string.Empty, 80, 80, _RecordContent);
        }
        #endregion

        #region Private Members
        private RText _RecordNumber;
        private RText _BOFDRoutingNumber;
        private RText _BOFDBusinessEndorsementDate;
        private RText _BOFDItemSequenceNumber;
        private RText _DepositAccountNumberatBOFD;
        private RText _BOFDDepositBranch;
        private RText _PayeeName;
        private RText _TruncationIndicator;
        private RText _BOFDConversionIndicator;
        private RText _BOFDCorrectionIndicator;
        private RText _UserField;
        private RText _Reserved;
        #endregion

        #region Properties
        public string RecordNumber
        {
            get { return this._RecordNumber.Value; }
        }
        public string BOFDRoutingNumber
        {
            get { return this._BOFDRoutingNumber.Value; }
        }
        public string BOFDBusinessEndorsementDate
        {
            get { return this._BOFDBusinessEndorsementDate.Value; }
        }

        public string BOFDItemSequenceNumber
        {
            get { return this._BOFDItemSequenceNumber.Value; }
        }

        public string DepositAccountNumberatBOFD
        {
            get { return this._DepositAccountNumberatBOFD.Value; }
        }

        public string BOFDDepositBranch
        {
            get { return this._BOFDDepositBranch.Value; }
        }

        public string PayeeName
        {
            get { return this._PayeeName.Value; }
        }

        public string TruncationIndicator
        {
            get { return this._TruncationIndicator.Value; }
        }

        public string BOFDConversionIndicator
        {
            get { return this._BOFDConversionIndicator.Value; }
        }

        public string BOFDCorrectionIndicator
        {
            get { return this._BOFDCorrectionIndicator.Value; }
        }

        public string UserField
        {
            get { return this._UserField.Value; }
        }

        public string Reserved
        {
            get { return this._Reserved.Value; }
        }

        #endregion
    }   
}
