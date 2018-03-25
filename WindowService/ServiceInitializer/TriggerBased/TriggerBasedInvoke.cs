using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BLW.Modules.WindowsService.ServiceInitializer.TriggerBased
{
    public class TriggerBasedInvoke
    {
        public TriggerBasedInvoke(string _exePath, string _triggerFilePath, string _guid)
        {
            this.exeFilePath = _exePath;
            this.triggerFilePath = _triggerFilePath;
            this.guid = _guid;
        }
        private string exeFilePath;
        private string guid;
        private string triggerFilePath;

        public string TriggerFilePath
        {
            get { return triggerFilePath; }
        }
        public string Guid
        {
            get { return guid; }
        }
        public string ExeFilePath
        {
            get { return exeFilePath; }
        }

        /// <summary>
        /// Invoke executable file
        /// </summary>
        public System.Diagnostics.Process Invoke()
        {
            System.Diagnostics.Process proces = new System.Diagnostics.Process();
            proces.StartInfo.FileName = exeFilePath;
            proces.StartInfo.Arguments = '"' + triggerFilePath + '"' + " " + '"' + guid + '"';
            proces.StartInfo.CreateNoWindow = true;
            proces.StartInfo.UseShellExecute = false;
            proces.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proces.Start();
            return proces;            
        }
    }
}
