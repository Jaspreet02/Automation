using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// Record 25
    /// </summary>
    [Serializable]
    public class R25
    {
        #region Ctor
        public R25()
        {
        }
        public R25(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 25 not valid - Invalid file.");
            _AuxiliaryOnUs = new RText("Auxiliary OnUs", string.Empty, 3, 17, _RecordContent);
            _ExternalProcessingCode = new RText("External Processing Code", string.Empty, 18, 18, _RecordContent);
            _PayorBankRoutingNumber = new RText("Payor Bank Routing Number", string.Empty, 19, 26, _RecordContent);
            _PayorBankRoutingNumberCheckDigit = new RText("Payor Bank Routing Number Check Digit", string.Empty, 27, 27, _RecordContent);
            _OnUs = new RText("OnUs", string.Empty, 28, 47, _RecordContent);
            _ItemAmount = new RText("Item Amount", string.Empty, 48, 57, _RecordContent);
            _ECEInstitutionItemSequenceNumber = new RText("ECE Institution Item Sequence Number", string.Empty, 58, 72, _RecordContent);
            _DocumentationTypeIndicator = new RText("Documentation Type Indicator", string.Empty, 73, 73, _RecordContent);
            _ReturnAcceptanceIndicator = new RText("Return Acceptance Indicator", string.Empty, 74, 74, _RecordContent);
            _MICRValidIndicator = new RText("MICR Valid Indicator", string.Empty, 75, 75, _RecordContent);
            _BOFDIndicator = new RText("BOFD Indicator", string.Empty, 76, 76, _RecordContent);
            _CheckDetailRecordAddendumCount = new RText("Check Detail Record Addendum Count", string.Empty, 77, 78, _RecordContent);
            _CorrectionIndicator = new RText("Correction Indicator", string.Empty, 79, 79, _RecordContent);
            _ArchiveTypeIndicator = new RText("Archive Type Indicator", string.Empty, 80, 80, _RecordContent);
            _FrontImageDetail = new R50();
            _FrontImage = new R52();
            _BackImageDetail = new R50();
            _BackImage = new R52();
            _AddendumAs = new List<R26>();
            _AddendumCs = new List<R28>();
        }
        #endregion

        #region Private Members
        private RText _AuxiliaryOnUs;
        private RText _ExternalProcessingCode;
        private RText _PayorBankRoutingNumber;
        private RText _PayorBankRoutingNumberCheckDigit;
        private RText _OnUs;
        private RText _ItemAmount;
        private RText _ECEInstitutionItemSequenceNumber;
        private RText _DocumentationTypeIndicator;
        private RText _ReturnAcceptanceIndicator;
        private RText _MICRValidIndicator;
        private RText _BOFDIndicator;
        private RText _CheckDetailRecordAddendumCount;
        private RText _CorrectionIndicator;
        private RText _ArchiveTypeIndicator;
        #endregion


        #region Properties

        private R50 _FrontImageDetail;

        public R50 FrontImageDetail
        {
            get { return _FrontImageDetail; }
            set { _FrontImageDetail = value; }
        }
        private R50 _BackImageDetail;

        public R50 BackImageDetail
        {
            get { return _BackImageDetail; }
            set { _BackImageDetail = value; }
        }
        private R52 _FrontImage;

        public R52 FrontImage
        {
            get { return _FrontImage; }
            set { _FrontImage = value; }
        }
        private R52 _BackImage;

        public R52 BackImage
        {
            get { return _BackImage; }
            set { _BackImage = value; }
        }
        private List<R26> _AddendumAs;
        private List<R28> _AddendumCs;

        public List<R26> AddendumAs
        {
            get { return _AddendumAs; }
            set { _AddendumAs = value; }
        }
        public List<R28> AddendumCs
        {
            get { return _AddendumCs; }
            set { _AddendumCs = value; }
        }

        public string AuxiliaryOnUs
        {
            get { return this._AuxiliaryOnUs.Value; }
        }

        public string ExternalProcessingCode
        {
            get { return this._ExternalProcessingCode.Value; }
        }

        public string PayorBankRoutingNumber
        {
            get { return this._PayorBankRoutingNumber.Value; }
        }

        public string PayorBankRoutingNumberCheckDigit
        {
            get { return this._PayorBankRoutingNumberCheckDigit.Value; }
        }

        public string OnUs
        {
            get { return this._OnUs.Value; }
        }

        public string ItemAmount
        {
            get { return this._ItemAmount.Value; }
        }

        public string ECEInstitutionItemSequenceNumber
        {
            get { return this._ECEInstitutionItemSequenceNumber.Value; }
        }

        public string DocumentationTypeIndicator
        {
            get { return this._DocumentationTypeIndicator.Value; }
        }

        public string ReturnAcceptanceIndicator
        {
            get { return this._ReturnAcceptanceIndicator.Value; }
        }

        public string MICRValidIndicator
        {
            get { return this._MICRValidIndicator.Value; }
        }

        public string BOFDIndicator
        {
            get { return this._BOFDIndicator.Value; }
        }

        public string CheckDetailRecordAddendumCount
        {
            get { return this._CheckDetailRecordAddendumCount.Value; }
        }

        public string CorrectionIndicator
        {
            get { return this._CorrectionIndicator.Value; }
        }

        public string ArchiveTypeIndicator
        {
            get { return this._ArchiveTypeIndicator.Value; }
        }

        #endregion
    }
}
