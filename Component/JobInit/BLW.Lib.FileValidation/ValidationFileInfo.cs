using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.FileValidation
{
    public class ValidationFileInfo
    {
        bool _required = false;
        string _mask = string.Empty;
        public ValidationFileInfo(bool required, string mask)
        {
            _required = required;
            _mask = mask;
        }
        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }        

        public string Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }
    }
}
