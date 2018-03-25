using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// Record 01 
    /// </summary>
    [Serializable]
    public class R01
    {
        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        public R01()
        {
            //Do nothing
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordB"></param>
        public R01(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 01 not valid - Invalid file.");
            _Standardlevel = new RText("Standard level", "03 Indicates DSTU X9.37 – 2003", 3, 4, _RecordContent);
            _TestFileIndicator = new RText("Test File Indicator", "Must be a valid routing and transit number issued by the ABA’s Routing Number Registrar. The routing number shall be mod-checked with a valid check digit", 5, 6, _RecordContent);
            _ImmediateDestinationRoutingNumber = new RText("Immediate Destination Routing Number", "Must be a valid routing and transit number issued by the ABA’s Routing Number Registrar. The routing number shall be mod-checked with a valid check digit", 6, 14, _RecordContent);
            _ImmediateOriginRoutingNumber = new RText("Immediate Origin Routing Number", "Must be a valid routing and transit number issued by the ABA’s Routing Number Registrar. The routing number shall be mod-checked with a valid check digit", 15, 23, _RecordContent);
            _FileCreationDate = new RText("File Creation Date", "Must be a valid date in YYYYMMDD format. Eastern time zone shall be used.", 24, 31, _RecordContent);
            _FileCreationTime = new RText("File Creation Time", "Must be military time in HHMM format. Eastern time zone shall be used", 32, 35, _RecordContent);
            _ResendIndicator = new RText("Resend Indicator", "", 36, 36, _RecordContent);
            _ImmediateDestinationName = new RText("Immediate Destination Name", "Expanded Field type to support greater variety of abbreviations", 37, 54, _RecordContent);
            _ImmediateOriginName = new RText("Immediate Origin Name", "Expanded Field type to support greater variety of abbreviations", 55, 72, _RecordContent);
            _FileIDModifier = new RText("File ID Modifier", "This field is used to uniquely identify files. Lower case letters are NOT valid. ID is required if fields 4, 5, 6 and 7 are equal on other files within a file creation date.", 73, 73, _RecordContent);
            _CountryCode = new RText("Country Code", "", 74, 76, _RecordContent);
            _UserField = new RText("User Field", "", 77, 79, _RecordContent);
            _CompanionDocumentVersionIndicator = new RText("Companion Document Version Indicator", "", 80, 80, _RecordContent);
            _CashLetterHeaders = new List<R10>();

        }
        #endregion

        #region Private Members
        private List<R10> _CashLetterHeaders;
        private RText _Standardlevel;
        private RText _TestFileIndicator;
        private RText _ImmediateDestinationRoutingNumber;
        private RText _ImmediateOriginRoutingNumber;
        private RText _FileCreationDate;
        private RText _FileCreationTime;
        private RText _ResendIndicator;
        private RText _ImmediateDestinationName;
        private RText _ImmediateOriginName;
        private RText _FileIDModifier;
        private RText _CountryCode;
        private RText _UserField;
        private RText _CompanionDocumentVersionIndicator;
        #endregion

        #region Properties
        /// <summary>
        /// Cash letter headers
        /// </summary>
        public List<R10> CashLetterHeaders
        {
            get { return _CashLetterHeaders; }
            set { _CashLetterHeaders = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Standardlevel
        {
            get { return this._Standardlevel.Value; }
        }
        public string TestFileIndicator
        {
            get { return this._TestFileIndicator.Value; }
        }
        public string ImmediateDestinationRoutingNumber
        {
            get { return this._ImmediateDestinationRoutingNumber.Value; }
        }
        public string ImmediateOriginRoutingNumber
        {
            get { return this._ImmediateOriginRoutingNumber.Value; }
        }
        public string FileCreationDate
        {
            get { return this._FileCreationDate.Value; }
        }
        public string FileCreationTime
        {
            get { return this._FileCreationTime.Value; }
        }
        public string ResendIndicator
        {
            get { return this._ResendIndicator.Value; }
        }
        public string ImmediateDestinationName
        {
            get { return this._ImmediateDestinationName.Value; }
        }
        public string ImmediateOriginName
        {
            get { return this._ImmediateOriginName.Value; }
        }
        public string FileIDModifier
        {
            get { return this._FileIDModifier.Value; }
        }
        public string CountryCode
        {
            get { return this._CountryCode.Value; }
        }
        public string UserField
        {
            get { return this._UserField.Value; }
        }
        public string CompanionDocumentVersionIndicator
        {
            get { return this._CompanionDocumentVersionIndicator.Value; }
        }
        #endregion
    }

}
