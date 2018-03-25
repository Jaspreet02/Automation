using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
 public class ErrorLog : BaseModel
    {
        public ErrorLog()
        {

        }
        public int ErrorLogId { get; set; }
        public string ComponentName { get; set; }
        public DateTime ErrorTime { get; set; }
        public string ErrorMessage { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string StrackTrace { get; set; }
        public int AppId { get; set; }
        public int CompId { get; set; }
        public int RunId { get; set; }
        public int AppCompId { get; set; }
        
        

    }
}
