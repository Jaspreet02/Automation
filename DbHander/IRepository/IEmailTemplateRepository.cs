using System;
using System.Linq.Expressions;

namespace DbHander
{
    public interface IEmailTemplateRepository : IGenericRepository<EmailTemplate>
    {
        EmailTemplate EmailTemplate(Expression<Func<EmailTemplate, bool>> predicate);
    }
}
