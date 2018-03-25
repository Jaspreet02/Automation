using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
  public class ProcComponent : BaseModel
    {
        public ProcComponent()
        {

        }
        public int ProcComponentId { get; set; }
        public string Name { get; set; }
        public string ExecutablePath { get; set; }
        public int LicenceLimit { get; set; }
        
        
        
    }
}
