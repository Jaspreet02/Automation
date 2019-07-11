using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DbHander
{
    public class EmailTracking : BaseModel
    {
        public EmailTracking()
        {
        }
        public int EmailTrackingId { get; set; }
        public int EmailTemplateId { get; set; }
        [ForeignKey("EmailTemplateId")]
        public EmailTemplate EmailTemplate { get; set; }
        public int RunNumberId { get; set; }
        [ForeignKey("RunNumberId")]
        public RunDetail RunNumber { get; set; }
        public string FromEmailId { get; set; }
        public string EmailToIds { get; set; }
        public string EmailCcIds { get; set; }
        public string Subjects { get; set; }
        public string Body { get; set; }
        public string SentMessage { get; set; }
        public int EmailStatus { get; set; }
        public DateTime SentDate { get; set; }       
    }
}
