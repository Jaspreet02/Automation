using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BLW.Modules.WindowsService.ServiceInitializer.TimeBased
{
    public class TimeIntervalBasedInvoke
    {
        public TimeIntervalBasedInvoke(string _exePath, string _guid)
        {
            this.exeFilePath = _exePath;
            this.guid = _guid;
        }
        private string exeFilePath;
        private string guid;

        public string Guid
        {
            get { return guid; }            
        }

        public string ExeFilePath
        {
            get { return exeFilePath; }    
        }

        public System.Diagnostics.Process Invoke()
        {
            System.Diagnostics.Process proces = new System.Diagnostics.Process();
            proces.StartInfo.FileName = exeFilePath;
            proces.StartInfo.Arguments = guid;
            proces.StartInfo.CreateNoWindow = true;
            proces.StartInfo.UseShellExecute = false;
            proces.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proces.Start();            
            return proces;
        }
    }
}
