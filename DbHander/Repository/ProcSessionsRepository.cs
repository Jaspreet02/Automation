using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public class ProcSessionsRepository : AbstractRepository, IProcSessionsRepository
    {
        /// <summary>
        /// Gets list of  unused process.
        /// </summary>
        /// <returns></returns>
        public List<ProcSession> GetAllUnusedProcess()
        {
                return objDataContext.ProcSessions.Where(x => x.KillRequired == true || x.ExpectedDateTime < DateTime.Now || x.ProcStatus == 2 || x.ProcStatus == 3).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProcSession> FindAll()
        {
                return objDataContext.ProcSessions;
        }

        public IQueryable<ProcSession> FindAllActive()
        {
                return objDataContext.ProcSessions;
}

        /// <summary>
        /// find the proc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProcSession Find(int id)
        {
            return objDataContext.ProcSessions.SingleOrDefault(x => x.ProcSessionId.Equals(id));
        }

        /// <summary>
        /// saves the proc session
        /// </summary>
        /// <param name="Procdao"></param>
        /// <returns></returns>
        public int Save(ProcSession dao)
        {
            ProcSession entity = objDataContext.ProcSessions.SingleOrDefault(x => x.ProcSessionId.Equals(dao.ProcSessionId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.ProcSessions.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ProcSessionId;
        }

        public bool Delete(int id)
        {
                var procSessionObj = objDataContext.ProcSessions.Where(x => x.ProcSessionId == id).SingleOrDefault();
                if (procSessionObj != null)
                    objDataContext.ProcSessions.Remove(procSessionObj);
                objDataContext.SaveChanges();
                return true;
         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateBySessionKey(string sessionKey, byte status)
        {
                var procSession = objDataContext.ProcSessions.Where(x => x.SessionKey == sessionKey).Select(x => x).FirstOrDefault();
                if (procSession != null)
                    procSession.ProcStatus = status;
                objDataContext.SaveChanges();
                return true;
         }
        
       
        public bool UpdateStatus(int id)
        {
            throw new NotImplementedException();
        }

        public bool CheckProcSessionLimit(int Limit, int procId)
        {
                var sessionDetail = objDataContext.ProcSessions.Where(x => x.ProcComponentID == procId && x.Status).Select(x => x);
                var count = sessionDetail.Count();
                if (count < Limit)
                    return true;
                else
                    return false;
        }

        public bool IsAppRunning(int appId)
        {
                return !objDataContext.ProcSessions.Any(x => x.ApplicationId.Equals(appId) && x.ProcStatus == (byte)JobStatusType.Running);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    objDataContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProcSessionsRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
