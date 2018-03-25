using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace BLW.Lib.FileTransfer
{
    public class FileTransferSettingOpration : IAdaperSettings
    {
        public string Type { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int Port { get; set; }
        public string SourceLocation { get; set; }
        public string DestinationLocation { get; set; }

        public List<String> GetFilesByMask(List<String> files, string mask)
        {
            List<String> list = new List<string>();
            var newfiles = files.Where(x => Regex.Match(Path.GetFileName(x).ToUpper(), mask.ToUpper()).Success).Select(x => x);
            list.AddRange(newfiles);
            return list;
        }
    }
}
