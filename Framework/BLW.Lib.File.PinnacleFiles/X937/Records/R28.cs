using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{    /// <summary>
    /// Record 28
    /// </summary>
    [Serializable]
    public class R28
    {
        #region Ctor
        public R28(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 28 not valid - Invalid file.");
            _RecordNumber = new RText(string.Empty, string.Empty, 3, 4, _RecordContent);
            _EndorsingBankRoutingNumber = new RText(string.Empty, string.Empty, 5, 13, _RecordContent);
            _EndorsingBankEndorsementDate = new RText(string.Empty, string.Empty, 14, 21, _RecordContent);
            _EndorsingBankItemSequenceNumber = new RText(string.Empty, string.Empty, 22, 36, _RecordContent);
            _TruncationIndicator = new RText(string.Empty, string.Empty, 37, 37, _RecordContent);
            _EndorsingBankConversionIndicator = new RText(string.Empty, string.Empty, 38, 38, _RecordContent);
            _EndorsingBankCorrectionIndicator = new RText(string.Empty, string.Empty, 39, 39, _RecordContent);
            _ReturnReason = new RText(string.Empty, string.Empty, 40, 40, _RecordContent);
            _UserField = new RText(string.Empty, string.Empty, 41, 59, _RecordContent);
            _EndorsingBankIdentifier = new RText(string.Empty, string.Empty, 60, 60, _RecordContent);
            _Reserved = new RText(string.Empty, string.Empty, 61, 80, _RecordContent);
        }
        #endregion

        #region Private Members
        private RText _RecordNumber;
        private RText _EndorsingBankRoutingNumber;
        private RText _EndorsingBankEndorsementDate;
        private RText _EndorsingBankItemSequenceNumber;
        private RText _TruncationIndicator;
        private RText _EndorsingBankConversionIndicator;
        private RText _EndorsingBankCorrectionIndicator;
        private RText _ReturnReason;
        private RText _UserField;
        private RText _EndorsingBankIdentifier;
        private RText _Reserved;
        #endregion


        #region Properties
        public string RecordNumber
        {
            get { return this._RecordNumber.Value; }
        }

        public string EndorsingBankRoutingNumber
        {
            get { return this._EndorsingBankRoutingNumber.Value; }
        }

        public string EndorsingBankEndorsementDate
        {
            get { return this._EndorsingBankEndorsementDate.Value; }
        }

        public string EndorsingBankItemSequenceNumber
        {
            get { return this._EndorsingBankItemSequenceNumber.Value; }
        }

        public string TruncationIndicator
        {
            get { return this._TruncationIndicator.Value; }
        }

        public string EndorsingBankConversionIndicator
        {
            get { return this._EndorsingBankConversionIndicator.Value; }
        }

        public string EndorsingBankCorrectionIndicator
        {
            get { return this._EndorsingBankCorrectionIndicator.Value; }
        }

        public string ReturnReason
        {
            get { return this._ReturnReason.Value; }
        }

        public string UserField
        {
            get { return this._UserField.Value; }
        }

        public string EndorsingBankIdentifier
        {
            get { return this._EndorsingBankIdentifier.Value; }
        }

        public string Reserved
        {
            get { return this._Reserved.Value; }
        }
        #endregion
    } 
}
