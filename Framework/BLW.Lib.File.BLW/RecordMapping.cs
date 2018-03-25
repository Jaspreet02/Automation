using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.File.BLW
{
    public class RecordMapping
    {        
        public RecordMapping(string field, int start, int length)
        {
            this._Field = field;
            this._Start = start;
            this._Length = length;
        }
        private string _Field;

        public string Field
        {
            get { return _Field; }
            set { _Field = value; }
        }
        private int _Start;

        public int Start
        {
            get { return _Start; }
            set { _Start = value; }
        }
        private int _Length;

        public int Length
        {
            get { return _Length; }
            set { _Length = value; }
        }

    }
}
