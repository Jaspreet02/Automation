using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{   
    /// <summary>
    /// Record 52
    /// </summary>
    [Serializable]
    public class R52
    {

        #region Ctor
        public R52()
        {
        }
        public R52(byte[] recordB, int currentPOS, bool isFront)
        {
            //Adding record counter (4 bytes length) to current Position
            currentPOS += 4;
            string _RecordContent = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB, 0, 105));
            _ECEInstitutionRoutingNumber = new RText(string.Empty, string.Empty, 3, 11, _RecordContent);
            _BundleBusinessDate = new RText(string.Empty, string.Empty, 12, 19, _RecordContent);
            _ECEInstitutionItemSequenceNumber = new RText(string.Empty, string.Empty, 22, 36, _RecordContent);
            _ClippingOrigin = new RText(string.Empty, string.Empty, 85, 85, _RecordContent);
            _LengthofImageReferenceKey = new RText(string.Empty, string.Empty, 102, 105, _RecordContent);
            ParseImageValues(recordB, currentPOS, isFront);
        }
        #endregion

        #region Private Members
        private RText _ECEInstitutionRoutingNumber;
        private RText _BundleBusinessDate;
        private RText _ECEInstitutionItemSequenceNumber;
        private RText _ClippingOrigin;
        private RText _LengthofImageReferenceKey;
        //Extra variable data
        private string _ImageReferenceKey;
        private string _LengthofDigitalSignature;
        private int _ImageStartIndex;
        private int _ImageLength;
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public void ParseImageValues(byte[] recordB, int currentPOS, bool isFront)
        {
            // length of image ref. key
            int refKeyLen = int.Parse(LengthofImageReferenceKey);
            //read image ref key and digital sig length
            string rec = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB, 0, 105 + refKeyLen + 5));
            _LengthofDigitalSignature = rec.Substring(105 + refKeyLen);
            int sigLen = int.Parse(_LengthofDigitalSignature);
            //read everything except image
            rec = Encoding.ASCII.GetString(Encoding.Convert(Encoding.GetEncoding(37), Encoding.GetEncoding("ASCII"), recordB, 0, 105 + refKeyLen + 5 + sigLen + 7));
            _ImageLength = int.Parse(rec.Substring(rec.Length - 7));
            _ImageStartIndex = currentPOS - (recordB.Length - 117);
            //Image Extraction          
            /*
            byte[] outArr = new byte[recordB.Length - rec.Length];
            Array.Copy(recordB, rec.Length, outArr, 0, recordB.Length - rec.Length);
            if (outArr != null)
                _CheckImage = this.Byte2Image(outArr);
            **/
        }
        /// <summary>
        /// Convert bytes to image
        /// </summary>
        /// <param name="ByteArr"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public Image Byte2Image(byte[] ByteArr)
        {
            Image NewImage = null;
            MemoryStream ImageStream = null;
            try
            {
                if (ByteArr != null)
                {
                    ImageStream = new MemoryStream(ByteArr);
                    NewImage = Image.FromStream(ImageStream);
                }
                else
                {
                    NewImage = null;
                }
            }
            catch (Exception)
            {
                NewImage = null;
            }
            return NewImage;
        }
        #endregion

        # region Proerties
        public string ECEInstitutionRoutingNumber
        {
            get { return this._ECEInstitutionRoutingNumber.Value; }
        }

        public string BundleBusinessDate
        {
            get { return this._BundleBusinessDate.Value; }
        }

        public string ECEInstitutionItemSequenceNumber
        {
            get { return this._ECEInstitutionItemSequenceNumber.Value; }
        }

        public string ClippingOrigin
        {
            get { return this._ClippingOrigin.Value; }
        }

        public string LengthofImageReferenceKey
        {
            get { return this._LengthofImageReferenceKey.Value; }
        }

        public string ImageReferenceKey
        {
            get { return this._ImageReferenceKey; }
        }
        public string LengthofDigitalSignature
        {
            get { return this._LengthofDigitalSignature; }
        }
        public int ImageStartIndex
        {
            get { return this._ImageStartIndex; }
        }
        public int ImageLength
        {
            get { return this._ImageLength; }
        }
        /// <summary>
        /// Define a check file name at the time of splitting file
        /// </summary>
        public string CheckFileName { get; set; }
        #endregion
    }       
}
