using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BLW.Lib.CoreUtility.Exceptions
{
    [Serializable]
    public class IOBLWException : System.IO.IOException
    {
        public IOBLWException()
            : base() { }

        public IOBLWException(string message)
            : base(message) { }

        public IOBLWException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public IOBLWException(string message, Exception innerException)
            : base(message, innerException) { }

        public IOBLWException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        public IOBLWException(string format, Exception innerException, string fileName, string module, params object[] args)
            : base(string.Format(format, args), innerException) {
                _FileName = fileName;
                _Module = module;
        }

        public override string ToString()
        {
            return string.Format("{0} is locked by another module. {1} throwing IOBLWException error.\n", _FileName, _Module) + base.ToString();
        }

        protected IOBLWException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }           
        }
        private string _Module;
        public string Module
        {
            get { return _Module; }
        }
    }
}

