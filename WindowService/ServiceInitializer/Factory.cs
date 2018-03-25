using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Modules.WindowsService.ServiceInitializer;
using BLW.Modules.WindowsService.ServiceInitializer.TriggerBased;
using BLW.Modules.WindowsService.ServiceInitializer.TimeBased;

namespace ConsoleApplication1
{
    class Factory
    {
        /// <summary>
        /// Create Class instance
        /// </summary>
        /// <param name="type">TYPE</param>
        /// <returns></returns>
        public IProduct GetInstance(string type)
        {
            IProduct instance = null; ;
            if(type.ToUpper() == "TRIGGERBASE")
            {
                instance = new TriggerBased();
            }
            else if(type.ToUpper() == "TIMEBASE")
            {
                instance = new TimeBased();
            }

            return instance;
        }
    }
}
