using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BLW.Lib.CoreUtility;

namespace BLW.Modules.WindowsService.ServiceInitializer.TriggerBased
{
    class TriggerFileOpration
    {
        public string TriggerFilePath { get; set; }
        public string ReadExcutableFileLocationFromTriigerFile()
        {
            TriggerFileReader triggerFileReaderObj = new TriggerFileReader();
            triggerFileReaderObj.TriggerFileLocaton = TriggerFilePath;

            var triggerFileInfo = triggerFileReaderObj.GetTriggerFileDetail();

            if (String.IsNullOrEmpty(triggerFileInfo.ComponentExe))
                throw new Exception("Executable file location not found in trigger file. Trigger file name is " + Path.GetFileName(TriggerFilePath));

            return triggerFileInfo.ComponentExe;
        }
    }
}
