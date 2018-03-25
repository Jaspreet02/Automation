using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace BLW.Lib.File.PinnacleFiles.X937
{
    /// <summary>
    /// Record Text
    /// </summary>
    [Serializable]
    public class RText
    {
        /// <summary>
        /// 
        /// </summary>
        public RText()
        {
            //Do nothing
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="record"></param>
        public RText(string name, string description, int start, int end, string record)
        {
            this._Name = name;
            this._Description = description;
            this._Start = start;
            this._End = end;
            this._Value = string.Empty;
            //get the value from the record             
            if (!string.IsNullOrEmpty(record))
                if (record.Length > 0)
                    this._Value = record.Substring(this.Begin, this.Length);
        }
        private int _Start;
        /// <summary>
        /// Start
        /// </summary>
        public int Start
        {
            get { return _Start; }
            set { _Start = value; }
        }
        public int Begin
        {
            get { return (_Start - 1); }
        }
        /// <summary>
        /// Length
        /// </summary>
        public int Length
        {
            get { return (_End - Begin); }
        }
        private int _End;
        /// <summary>
        /// End
        /// </summary>
        public int End
        {
            get { return _End; }
            set { _End = value; }
        }

        private string _Value;
        /// <summary>
        /// Value
        /// </summary>
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
    }  
}
