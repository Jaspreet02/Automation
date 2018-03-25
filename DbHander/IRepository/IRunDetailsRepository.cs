using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public interface IRunDetailsRepository : IGenericRepository<RunDetail>
    {
        IQueryable<RunDetail> GetRunDetailByStatus(int status);
        RunDetail GetRunDetailByRunNumber(string runNumber);
        int GetApplicationIdByRunNumber(String runNumber);
        int GetRunNumberIdByRunNumber(String runNumber);
        bool UpdateRunStatusByRunNumberId(int runNumberId, byte UpdatedValue);
        bool GetAppComponentTypeByRunCompStatustId(int RuncompStatusId, int appId);
        string GetLastRunNumber(int appId);
    }
}
