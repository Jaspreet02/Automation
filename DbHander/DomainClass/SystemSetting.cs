using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class SystemSetting : BaseModel
    {
        public SystemSetting()
        {

        }
        public int SystemSettingId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        
        
    }
}
