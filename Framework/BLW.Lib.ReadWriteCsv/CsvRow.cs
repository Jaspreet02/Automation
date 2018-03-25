using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace BLW.Lib.ReadWriteCsv
{
    /// <summary>
    /// Class to store one CSV row
    /// </summary>DynamicObject

    public class CsvLine : DynamicObject
    {
        string[] _lineContent;
        List<string> _headers;

        public CsvLine(string line, List<string> headers)
        {
            this._lineContent = line.Split(',');
            this._headers = headers;
        }

        public override bool TryGetMember(
            GetMemberBinder binder,
            out object result)
        {
            result = null;

            // find the index position and get the value
            int index = _headers.IndexOf(binder.Name);
            if (index >= 0 && index < _lineContent.Length)
            {
                result = _lineContent[index];
                return true;
            }

            return false;
        }
    }

    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }
}
