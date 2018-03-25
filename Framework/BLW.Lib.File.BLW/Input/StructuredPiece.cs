using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.BLW.Input
{
    public class StructuredPiece
    {
        public RText RecordIndexNumber { get; set; }
        public RText AppCode { get; set; }
        public RText JobId { get; set; }
        public RText ReferenceNumber { get; set; }
        public RText Name1 { get; set; }
        public RText Name2 { get; set; }
        public RText Name3 { get; set; }
        public RText Name4 { get; set; }
        public RText Address1 { get; set; }
        public RText Address2 { get; set; }
        public RText Address3 { get; set; }
        public RText Address4 { get; set; }        
        public RText Address5 { get; set; }
        public RText City { get; set; }
        public RText State { get; set; }
        public RText Zip { get; set; }
        public RText ReturnAddress1 { get; set; }
        public RText ReturnAddress2 { get; set; }
        public RText ReturnAddress3 { get; set; }
        public RText ReturnAddress4 { get; set; }
        public RText ReturnAddress5 { get; set; }
        public RText ReturnCity { get; set; }
        public RText ReturnState { get; set; }
        public RText ReturnZip { get; set; }
        public RText Tray1 { get; set; }
        public RText Tray2 { get; set; }
        public RText Tray3 { get; set; }
        public RText Tray4 { get; set; }
        public RText Tray5 { get; set; }
        public RText Tray6 { get; set; }
        public RText SupressionCode { get; set; }
        public RText Images { get; set; }
        public RText PhysicalPages { get; set; }
        public RText UDF1 { get; set; }
        public RText UDF2 { get; set; }
        public RText UDF3 { get; set; }
        public RText UDF4 { get; set; }
        public RText UDF5 { get; set; }
        public RText UDF6 { get; set; }
        public RText UDF7 { get; set; }
        public RText UDF8 { get; set; }
        public RText UDF9 { get; set; }
        public RText UDF10 { get; set; }
        public RText PostalProcessing { get; set; }
        public RText DeliveryMode { get; set; }
        public RText DeliveryLocation { get; set; }
        public RText PackageType { get; set; }
        public RText Insert1 { get; set; }
        public RText Insert2 { get; set; }
        public RText Insert3 { get; set; }
        public RText Insert4 { get; set; }
        public RText Insert5 { get; set; }
        public RText Insert6 { get; set; }
        public RText Insert7 { get; set; }
        public RText Insert8 { get; set; }
        public RText PackageWeight { get; set; }
        public RText ValidRecord { get; set; }
        public RText SampleRecord { get; set; }
        public RText ProcessingDate { get; set; }
        public RText EpresentmentCode { get; set; }
        public RText ClientId { get; set; }
        public RText Plexing { get; set; }
        public RText LetDate { get; set; }
        public RText ProjectName { get; set; }
        public RText PageStart { get; set; }
        public RText PageEnd { get; set; }
        public RText EpresentmentInd { get; set; }
        public RText FileDate { get; set; }
        
        public StructuredPiece(string line, IEnumerable<RecordMapping> mapping)
        {
            RecordIndexNumber = new RText(line, mapping, Constants.InputFileField.RECORD_INDEX_NUMBER, FormatTypes.RemovePrefixZeros);
            AppCode = new RText(line, mapping, Constants.InputFileField.AppCode, FormatTypes.RemovePrefixZeros);
            JobId = new RText(line, mapping, Constants.InputFileField.JobId, FormatTypes.RemovePrefixZeros);
            ReferenceNumber = new RText(line, mapping, Constants.InputFileField.ReferenceNumber, FormatTypes.None);
            Name1 = new RText(line, mapping, Constants.InputFileField.Name1, FormatTypes.RemovePrefixZeros);
            Name2 = new RText(line, mapping, Constants.InputFileField.Name2, FormatTypes.RemovePrefixZeros);
            Name3 = new RText(line, mapping, Constants.InputFileField.Name3, FormatTypes.RemovePrefixZeros);
            Name4 = new RText(line, mapping, Constants.InputFileField.Name4, FormatTypes.RemovePrefixZeros);
            Address1 = new RText(line, mapping, Constants.InputFileField.Address1, FormatTypes.RemovePrefixZeros);
            Address2 = new RText(line, mapping, Constants.InputFileField.Address2, FormatTypes.RemovePrefixZeros);
            Address3 = new RText(line, mapping, Constants.InputFileField.Address3, FormatTypes.RemovePrefixZeros);
            Address4 = new RText(line, mapping, Constants.InputFileField.Address4, FormatTypes.RemovePrefixZeros);
            Address5 = new RText(line, mapping, Constants.InputFileField.Address5, FormatTypes.RemovePrefixZeros);
            City = new RText(line, mapping, Constants.InputFileField.City, FormatTypes.RemovePrefixZeros);
            State = new RText(line, mapping, Constants.InputFileField.State, FormatTypes.RemovePrefixZeros);
            Zip = new RText(line, mapping, Constants.InputFileField.Zip, FormatTypes.RemovePrefixZeros);
            ReturnAddress1 = new RText(line, mapping, Constants.InputFileField.ReturnAdd1, FormatTypes.RemovePrefixZeros);
            ReturnAddress2 = new RText(line, mapping, Constants.InputFileField.ReturnAdd2, FormatTypes.RemovePrefixZeros);
            ReturnAddress3 = new RText(line, mapping, Constants.InputFileField.ReturnAdd3, FormatTypes.RemovePrefixZeros);
            ReturnAddress4 = new RText(line, mapping, Constants.InputFileField.ReturnAdd4, FormatTypes.RemovePrefixZeros);
            ReturnAddress5 = new RText(line, mapping, Constants.InputFileField.ReturnAdd5, FormatTypes.RemovePrefixZeros);
            ReturnCity = new RText(line, mapping, Constants.InputFileField.ReturnCity, FormatTypes.RemovePrefixZeros);
            ReturnState = new RText(line, mapping, Constants.InputFileField.ReturnState, FormatTypes.RemovePrefixZeros);
            ReturnZip = new RText(line, mapping, Constants.InputFileField.ReturnZip, FormatTypes.RemovePrefixZeros);
            Tray1 = new RText(line, mapping, Constants.InputFileField.Tray1, FormatTypes.RemovePrefixZeros);
            Tray2 = new RText(line, mapping, Constants.InputFileField.Tray2, FormatTypes.RemovePrefixZeros);
            Tray3 = new RText(line, mapping, Constants.InputFileField.Tray3, FormatTypes.RemovePrefixZeros);
            Tray4 = new RText(line, mapping, Constants.InputFileField.Tray4, FormatTypes.RemovePrefixZeros);
            Tray5 = new RText(line, mapping, Constants.InputFileField.Tray5, FormatTypes.RemovePrefixZeros);
            Tray6 = new RText(line, mapping, Constants.InputFileField.Tray6, FormatTypes.RemovePrefixZeros);
            Images = new RText(line, mapping, Constants.InputFileField.Images, FormatTypes.RemovePrefixZeros);
            PhysicalPages = new RText(line, mapping, Constants.InputFileField.PhysicalPages, FormatTypes.RemovePrefixZeros);
            UDF1 = new RText(line, mapping, Constants.InputFileField.UDF1, FormatTypes.RemovePrefixZeros);
            UDF2 = new RText(line, mapping, Constants.InputFileField.UDF2, FormatTypes.RemovePrefixZeros);
            UDF3 = new RText(line, mapping, Constants.InputFileField.UDF3, FormatTypes.RemovePrefixZeros);
            UDF4 = new RText(line, mapping, Constants.InputFileField.UDF4, FormatTypes.RemovePrefixZeros);
            UDF5 = new RText(line, mapping, Constants.InputFileField.UDF5, FormatTypes.RemovePrefixZeros);
            UDF6 = new RText(line, mapping, Constants.InputFileField.UDF6, FormatTypes.RemovePrefixZeros);
            UDF7 = new RText(line, mapping, Constants.InputFileField.UDF7, FormatTypes.RemovePrefixZeros);
            UDF8 = new RText(line, mapping, Constants.InputFileField.UDF8, FormatTypes.RemovePrefixZeros);
            UDF9 = new RText(line, mapping, Constants.InputFileField.UDF9, FormatTypes.RemovePrefixZeros);
            UDF10 = new RText(line, mapping, Constants.InputFileField.UDF10, FormatTypes.RemovePrefixZeros);
            PostalProcessing = new RText(line, mapping, Constants.InputFileField.PostalProcessing, FormatTypes.RemovePrefixZeros);
            DeliveryMode = new RText(line, mapping, Constants.InputFileField.DeliveryMode, FormatTypes.RemovePrefixZeros);
            DeliveryLocation = new RText(line, mapping, Constants.InputFileField.DeliveryLocation, FormatTypes.RemovePrefixZeros);
            ProcessingDate = new RText(line, mapping, Constants.InputFileField.ProcessingDate, FormatTypes.None);
            PackageType = new RText(line, mapping, Constants.InputFileField.PackageType, FormatTypes.RemovePrefixZeros);
            Insert1 = new RText(line, mapping, Constants.InputFileField.Insert1, FormatTypes.RemovePrefixZeros);
            Insert2 = new RText(line, mapping, Constants.InputFileField.Insert2, FormatTypes.RemovePrefixZeros);
            Insert3 = new RText(line, mapping, Constants.InputFileField.Insert3, FormatTypes.RemovePrefixZeros);
            Insert4 = new RText(line, mapping, Constants.InputFileField.Insert4, FormatTypes.RemovePrefixZeros);
            Insert5 = new RText(line, mapping, Constants.InputFileField.Insert5, FormatTypes.RemovePrefixZeros);
            Insert6 = new RText(line, mapping, Constants.InputFileField.Insert6, FormatTypes.RemovePrefixZeros);
            Insert7 = new RText(line, mapping, Constants.InputFileField.Insert7, FormatTypes.RemovePrefixZeros);
            Insert8 = new RText(line, mapping, Constants.InputFileField.Insert8, FormatTypes.RemovePrefixZeros);
            PackageWeight = new RText(line, mapping, Constants.InputFileField.PackageWeight, FormatTypes.RemovePrefixZeros);
            SupressionCode = new RText(line, mapping, Constants.InputFileField.SupressionCode, FormatTypes.RemovePrefixZeros);
            ValidRecord = new RText(line, mapping, Constants.InputFileField.ValidRecord, FormatTypes.RemovePrefixZeros);
            SampleRecord = new RText(line, mapping, Constants.InputFileField.SampleRecord, FormatTypes.RemovePrefixZeros);
            EpresentmentCode = new RText(line, mapping, Constants.InputFileField.EpresentmentCode, FormatTypes.RemovePrefixZeros);             
            ClientId = new RText(line, mapping, Constants.InputFileField.ClientId, FormatTypes.RemovePrefixZeros);             
            Plexing = new RText(line, mapping, Constants.InputFileField.Plexing, FormatTypes.RemovePrefixZeros);
            LetDate = new RText(line, mapping, Constants.InputFileField.LetDate, FormatTypes.None);             
            ProjectName = new RText(line, mapping, Constants.InputFileField.ProjectName, FormatTypes.RemovePrefixZeros);             
            PageStart = new RText(line, mapping, Constants.InputFileField.PageStart, FormatTypes.RemovePrefixZeros);             
            PageEnd = new RText(line, mapping, Constants.InputFileField.PageEnd, FormatTypes.RemovePrefixZeros);
            EpresentmentInd = new RText(line, mapping, Constants.InputFileField.EpresentmentInd, FormatTypes.RemovePrefixZeros);
            FileDate = new RText(line, mapping, Constants.InputFileField.FILE_DATE, FormatTypes.None);            
        }


       

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} : {1} {2}", "RecordIndexNumber", RecordIndexNumber.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "AppCode", AppCode.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "JobId", JobId.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2} ", "ReferenceNumber", ReferenceNumber.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Name1", Name1.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Name2", Name2.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Name3", Name3.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Name4", Name4.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Address1", Address1.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Address2", Address2.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Address3", Address3.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Address4", Address4.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Address5", Address5.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "City", City.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "State", State.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Zip", Zip.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ReturnAddress1", ReturnAddress1.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ReturnAddress2", ReturnAddress2.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ReturnAddress3", ReturnAddress3.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ReturnAddress4", ReturnAddress4.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ReturnAddress5", ReturnAddress5.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ReturnCity", ReturnCity.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ReturnState", ReturnState.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ReturnZip", ReturnZip.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Tray1", Tray1.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Tray2", Tray2.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Tray3", Tray3.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Tray4", Tray4.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Tray5", Tray5.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Tray6", Tray6.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Images", Images.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "PhysicalPages", PhysicalPages.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF1", UDF1.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF2", UDF2.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF3", UDF3.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF4", UDF4.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF5", UDF5.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF6", UDF6.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF7", UDF7.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF8", UDF8.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF9", UDF9.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "UDF10", UDF10.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "PostalProcessing", PostalProcessing.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "DeliveryMode", DeliveryMode.Value);
            sb.AppendFormat("{0} : {1} {2}", "DeliveryLocation", DeliveryLocation.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ProcessingDate", ProcessingDate.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "PackageType", PackageType.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Insert1", Insert1.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Insert2", Insert2.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Insert3", Insert3.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Insert4", Insert4.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Insert5", Insert5.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Insert6", Insert6.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Insert7", Insert7.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "Insert8", Insert8.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "PackageWeight", PackageWeight.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "SupressionCode", SupressionCode.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "ValidRecord", ValidRecord.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "SampleRecord", SampleRecord.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "EpresentmentInd", EpresentmentInd.Value, Constants.NEW_LINE);
            sb.AppendFormat("{0} : {1} {2}", "File Date", FileDate.Value, Constants.NEW_LINE);
            return sb.ToString();
        }
    }


}
