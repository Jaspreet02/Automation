using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHander
{
   public class AppCommandArgument : BaseModel
    {
        public AppCommandArgument()
        {

        }
       public int AppCommandArgumentId { get; set; }
       public int AppSchedulerId { get; set; }
       public int Order { get; set; }
       public string Argument { get; set; } 

    }
}
