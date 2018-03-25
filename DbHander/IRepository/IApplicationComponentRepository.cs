using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{    /// <summary>
    /// Application Component
    /// </summary>
    public interface IApplicationComponentRepository : IGenericRepository<ApplicationComponent>
    {
        IQueryable<ApplicationComponent> GetApplicationComponentListbyappId(int applicationId);
    }
}
