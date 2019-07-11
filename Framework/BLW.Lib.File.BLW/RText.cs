using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BLW.Lib.File.BLW
{
    /// <summary>
    /// Record Text - 
    /// It contain recod offset, length which help in getting value
    /// also this is centrize class which apply all the formating
    /// </summary>
    [Serializable]
    public class RText
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="start">start position</param>
        /// <param name="length">length</param>
        /// <param name="record">record line</param>
        /// <param name="format">format</param>
        public void Parse(string record, int start = -1, int length = -1, FormatTypes format = FormatTypes.None)
        {
            string value = string.Empty;
            var lineLength = start + length;
            this._Start = start;
            this._Length = length;

            //get the value from the record             
            if (!string.IsNullOrEmpty(record))
            {
                //If start position and length is -1 then no need to get value, just need to apply formatting
                if (start == -1 && length == -1)
                    value = record;
                else
                {
                    if (record.Length >= lineLength)
                        value = record.Substring(this.Start, this.Length);
                }
            }
            ///Format according to format type provided
            switch (format)
            {
                //date format
                case FormatTypes.DateFormat:
                    value = Utility.FormatDate(value);
                    break;
                //remove prefix zeros
                case FormatTypes.RemovePrefixZeros:
                    value = Utility.ReplacePrefixZeros(value);
                    break;
                //Do nothing
                case FormatTypes.None:
                default:
                    //Do nothing
                    break;
            }
            this._Value = value.Trim();
        }


        public RText(string record, IEnumerable<RecordMapping> mapping, string propName, FormatTypes format = FormatTypes.None)
        {
            int start = 0;
            int length = 0;
            if (mapping != null)
                foreach (var item in mapping)
                {
                    if (item.Field.ToLower().Equals(propName))
                    {
                        _Offset = Convert.ToInt32(item.Start);
                        start = Convert.ToInt32(item.Start) - 1;
                        length = Convert.ToInt32(item.Length);
                        break;
                    }
                }
            Parse(record, start, length, format);
        }

        #region Private Members
        private int _Start;
        private int _Offset;
        private int _Length;
        private string _Value;

        #endregion

        #region Properties
        /// <summary>
        /// Start
        /// </summary>
        public int Start
        {
            get { return _Start; }
            set { _Start = value; }
        }

        public int Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }
        /// <summary>
        ///Length
        /// </summary>
        public int Length
        {
            get { return _Length; }
            set { _Length = value; }
        }
        /// <summary>
        /// Value
        /// </summary>

        public virtual string Value
        {
            get
            {
#if DEBUG
                return string.IsNullOrEmpty(_Value) ? string.Empty : _Value;
#else
                return _Value;
#endif
            }
            set { _Value = value; }
        }
        #endregion

    }
    [Serializable]
    public class RTextInt : RText
    {
        public RTextInt(string record, IEnumerable<RecordMapping> mapping, string propName, FormatTypes format = FormatTypes.None)
            : base(record, mapping, propName, format)
        {
        }
        public int Value
        {

            get
            {
        #if DEBUG
                        return string.IsNullOrEmpty(base.Value) ? 0 : Convert.ToInt32(base.Value);
        #else
              return Convert.ToInt32(base.Value);
        #endif
            }
            set { base.Value = value.ToString(); }
        }
    }
    [Serializable]
    public class RTextDate : RText
    {
        public RTextDate(string record, IEnumerable<RecordMapping> mapping, string propName, FormatTypes format = FormatTypes.None)
            : base(record, mapping, propName, format)
        {
        }
        public DateTime Value
        {
            get
            {
#if DEBUG
                return string.IsNullOrEmpty(base.Value) ? DateTime.Now : Convert.ToDateTime(base.Value);
#else
                return Convert.ToDateTime(base.Value);
#endif
            }
            set { base.Value = value.ToString(); }
        }
    }

    [Serializable]
    public class RTextBool : RText
    {
        public RTextBool(string record, IEnumerable<RecordMapping> mapping, string propName, FormatTypes format = FormatTypes.None)
            : base(record, mapping, propName, format)
        {
        }
        public bool Value
        {
            get
            {
#if DEBUG
                return string.IsNullOrEmpty(base.Value) ? false : Convert.ToBoolean(base.Value);
#else
                return base.Value == "1";
#endif
            }
            set { base.Value = value.ToString(); }
        }
    }
}
