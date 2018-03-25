using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbHander;
using DbHander;

namespace DbHander
{
   public interface IEmailTemplateRepository : IGenericRepository<EmailTemplate>
    {
        IEnumerable<EmailTemplate> GetEmailTemplatesByKeyword(int? clientId = null, int? applicationId = null, int? appComponentId = null, string token = null,int? levelId = null);
        bool DeleteAndResetLevel(int templateId);
    }
}
