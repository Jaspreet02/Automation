using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class DeliveryEmailSetting : BaseModel
    {
        public DeliveryEmailSetting()
        {

        }
       public int DeliveryEmailSettingId { get; set; }
       public int ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        public string MaskName { get; set; }
       public string Mask { get; set; }
       public string InputPath { get; set; }
       public bool IsAttachment { get; set; }
        
        
        
    }
}
