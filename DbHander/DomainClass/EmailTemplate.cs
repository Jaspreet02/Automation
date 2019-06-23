using System.ComponentModel.DataAnnotations;

namespace DbHander
{
    public class EmailTemplate : BaseModel
    {
        public EmailTemplate()
        {
        }
        public int EmailTemplateId { get; set; }
        public int EmailFromSmtpId { get; set; }
        [Required]
        public string EmailToIds { get; set; }
        [Required]
        [StringLength(50)]
        public string Subject { get; set; }
        public int EmailLevelId { get; set; }
        public string EmailCcIds { get; set; }
        [Required]
        public string Body{ get; set; }
        [Required]
        public string EmailToken { get; set; }
        public int ClientId { get; set; }
        public int ApplicationId { get; set; }
        public int ApplicationComponentId { get; set; }
        public int TimeInterval { get; set; }      
    }
}
