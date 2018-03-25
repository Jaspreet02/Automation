using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.TransferManager.Setting
{
    public interface IAdaperSetting
    {
        string Type { get; set; }
        string Host { get; set; }
        string Password { get; set; }
        string UserName { get; set; }
        int Port { get; set; }
        string SourceLocation { get; set; }
        string DestinationLocation { get; set; }
    }
}
