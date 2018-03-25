using System.Collections.Generic;

namespace DbHander
{
    public interface IProcSessionsRepository : IGenericRepository<ProcSession>
    {
        List<ProcSession> GetAllUnusedProcess();
        bool UpdateBySessionKey(string sessionKey,byte status);
        bool CheckProcSessionLimit(int Limit, int procId);
        bool IsAppRunning(int appId);
    }
}
