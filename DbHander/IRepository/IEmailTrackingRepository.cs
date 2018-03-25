using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbHander;
using DbHander;

namespace DbHander
{
    public interface IEmailTrackingRepository : IGenericRepository<EmailTracking>
    {
        IEnumerable<EmailTracking> GetEmailTrackingByKeyword(int runNumberId);
        IQueryable<EmailTracking> GetReadyEmails(int status);
    }
}
