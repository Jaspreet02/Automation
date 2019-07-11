using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbHander;

namespace DbHander
{
    /// <summary>
    /// Application Component
    /// </summary>
    public interface ITriggerandStatusFileRepository : IGenericRepository<TriggerandStatusFile>
    {
        TriggerandStatusFile FindbyComponentId(int componentId);
    }
}
