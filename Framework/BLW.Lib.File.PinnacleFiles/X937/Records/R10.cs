using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// Record 10
    /// </summary>
    [Serializable]
    public class R10
    {
        #region Ctor
        public R10()
        {
        }
        public R10(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 10 not valid - Invalid file.");
            _CollectionTypeIndicator = new RText("Collection Type Indicator", "The only valid values are: '01' indicates Forward Presentment '03' indicates Return", 3, 4, _RecordContent);
            _DestinationRoutingNumber = new RText("Destination Routing Number", "Must be a valid routing and transit number issued by the ABA’s Routing Number Registrar. The routing number shall be mod-checked with a valid check digit", 5, 13, _RecordContent);
            _ECEInstitutionRoutingNumber = new RText("ECE Institution Routing Number", "Must be a valid routing and transit number issued by the ABA’s Routing Number Registrar. The routing number shall be mod-checked with a valid check digit", 14, 22, _RecordContent);
            _CashLetterBusinessDate = new RText("Cash Letter Business Date", "Must be a valid date in YYYYMMDD format", 23, 30, _RecordContent);
            _CashLetterCreationDate = new RText("Cash Letter Creation Date", "Must be a valid date in YYYYMMDD format. Eastern time zone shall be used.", 31, 38, _RecordContent);
            _CashLetterCreationTime = new RText("Cash Letter Creation Time", "hhmm cash letter was created. Eastern time zone shall be used.", 39, 42, _RecordContent);
            _CashLetterRecordTypeIndicator = new RText("Cash Letter Record Type Indicator", string.Empty, 43, 43, _RecordContent);
            _CashLetterDocumentationTypeIndicator = new RText("Cash Letter Documentation Type Indicator", string.Empty, 44, 44, _RecordContent);
            _CashLetterID = new RText("Cash Letter ID", string.Empty, 45, 52, _RecordContent);
            _OriginatorContactName = new RText("Originator Contact Name", string.Empty, 53, 66, _RecordContent);
            _OriginatorContactPhoneNumber = new RText("Originator Contact Phone Number", string.Empty, 67, 76, _RecordContent);
            _FedWorkType = new RText("Fed Work Type", string.Empty, 77, 77, _RecordContent);
            _ReturnsIndicator = new RText("Returns Indicator", string.Empty, 78, 78, _RecordContent);
            _UserField = new RText("User Field", string.Empty, 79, 79, _RecordContent);
            _Reserved = new RText("Reserved", string.Empty, 80, 80, _RecordContent);
            _Bundles = new List<R20>();
        }

        #endregion

        #region Private Members
        private List<R20> _Bundles;
        private RText _CollectionTypeIndicator;
        private RText _DestinationRoutingNumber;
        private RText _ECEInstitutionRoutingNumber;
        private RText _CashLetterBusinessDate;
        private RText _CashLetterCreationDate;
        private RText _CashLetterCreationTime;
        private RText _FileCreationTime;
        private RText _ResendIndicator;
        private RText _CashLetterRecordTypeIndicator;
        private RText _CashLetterDocumentationTypeIndicator;
        private RText _CashLetterID;
        private RText _OriginatorContactName;
        private RText _OriginatorContactPhoneNumber;
        private RText _FedWorkType;
        private RText _ReturnsIndicator;
        private RText _UserField;
        private RText _Reserved;

        #endregion

        #region Properties
        public List<R20> Bundles
        {
            get { return _Bundles; }
            set { _Bundles = value; }
        }
        public string CollectionTypeIndicator
        {
            get { return this._CollectionTypeIndicator.Value; }
        }

        public string DestinationRoutingNumber
        {
            get { return this._DestinationRoutingNumber.Value; }
        }

        public string ECEInstitutionRoutingNumber
        {
            get { return this._ECEInstitutionRoutingNumber.Value; }
        }

        public string CashLetterBusinessDate
        {
            get { return this._CashLetterBusinessDate.Value; }
        }

        public string CashLetterCreationDate
        {
            get { return this._CashLetterCreationDate.Value; }
        }

        public string CashLetterCreationTime
        {
            get { return this._CashLetterCreationTime.Value; }
        }

        public string CashLetterRecordTypeIndicator
        {
            get { return this._CashLetterRecordTypeIndicator.Value; }
        }

        public string CashLetterDocumentationTypeIndicator
        {
            get { return this._CashLetterDocumentationTypeIndicator.Value; }
        }

        public string CashLetterID
        {
            get { return this._CashLetterID.Value; }
        }

        public string OriginatorContactName
        {
            get { return this._OriginatorContactName.Value; }
        }

        public string OriginatorContactPhoneNumber
        {
            get { return this._OriginatorContactPhoneNumber.Value; }
        }

        public string FedWorkType
        {
            get { return this._FedWorkType.Value; }
        }

        public string ReturnsIndicator
        {
            get { return this._ReturnsIndicator.Value; }
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
