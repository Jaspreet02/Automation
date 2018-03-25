using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
  public  class GMCCommand : BaseModel
    {
        public GMCCommand()
        {

        }
        public int GMCCommandId { get; set; }
        public int ComponentId { get; set; }
        public int ApplicationId { get; set; }
        public int CommandOrder { get; set; }
        public string CommandKey { get; set; }
        public string CommandValue { get; set; }
        public int GmcAddonSchenerioId { get; set; }
        
        
    }
}
