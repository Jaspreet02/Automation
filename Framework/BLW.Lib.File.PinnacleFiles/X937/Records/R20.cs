using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// Record 20
    /// </summary>
    [Serializable]
    public class R20
    {
        #region Ctor
        public R20(byte[] recordB)
        {
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB));
            if (_RecordContent == null)
                throw new Exception("Record type 20 not valid - Invalid file.");
            _CollectionTypeIndicator = new RText("Collection Type Indicator", string.Empty, 3, 4, _RecordContent);
            _DestinationRoutingNumber = new RText("Destination Routing Number", string.Empty, 5, 13, _RecordContent);
            _ECEInstitutionRoutingNumber = new RText("ECE Institution Routing Number", string.Empty, 14, 22, _RecordContent);
            _BundleBusinessDate = new RText("Bundle Business Date", string.Empty, 23, 30, _RecordContent);
            _BundleCreationDate = new RText("Bundle Creation Date", string.Empty, 31, 38, _RecordContent);
            _BundleID = new RText("Bundle ID", string.Empty, 39, 48, _RecordContent);
            _BundleSequenceNumber = new RText("Bundle Sequence Number", string.Empty, 49, 52, _RecordContent);
            _CycleNumber = new RText("Cycle Number", string.Empty, 53, 54, _RecordContent);
            _ReturnLocationRoutingNumber = new RText("Return Location Routing Number", string.Empty, 55, 63, _RecordContent);
            _UserField = new RText("User Field", string.Empty, 64, 79, _RecordContent);
            _Reserved = new RText("Reserved", string.Empty, 80, 80, _RecordContent);
            _Checks = new List<R25>();
        }
        #endregion

        #region Private Members
        private RText _CollectionTypeIndicator;
        private RText _DestinationRoutingNumber;
        private RText _ECEInstitutionRoutingNumber;
        private RText _BundleBusinessDate;
        private RText _BundleCreationDate;
        private RText _BundleID;
        private RText _BundleSequenceNumber;
        private RText _CycleNumber;
        private RText _ReturnLocationRoutingNumber;
        private RText _UserField;
        private RText _Reserved;

        #endregion

        #region Properties

        private List<R25> _Checks;

        public List<R25> Checks
        {
            get { return _Checks; }
            set { _Checks = value; }
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

        public string BundleBusinessDate
        {
            get { return this._BundleBusinessDate.Value; }
        }

        public string BundleCreationDate
        {
            get { return this._BundleCreationDate.Value; }
        }

        public string BundleID
        {
            get { return this._BundleID.Value; }
        }

        public string BundleSequenceNumber
        {
            get { return this._BundleSequenceNumber.Value; }
        }

        public string CycleNumber
        {
            get { return this._CycleNumber.Value; }
        }

        public string ReturnLocationRoutingNumber
        {
            get { return this._ReturnLocationRoutingNumber.Value; }
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
