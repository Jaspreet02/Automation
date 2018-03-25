using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.TransferManager.Setting
{
   public class FileTransferSettings : IAdaperSetting
    {
        public string Type { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int Port { get; set; }
        public string SourceLocation { get; set; }
        public string DestinationLocation { get; set; }
    }
}
