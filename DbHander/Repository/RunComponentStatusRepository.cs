using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public  class RunComponentStatusRepository :AbstractRepository, IRunComponentStatusRepository
    {       
       public List<RunComponentStatus> GetRunComponentStatusbyRunId(Int32 runId)
       {
           List<RunComponentStatus> list = new List<RunComponentStatus>();
           
               list = objDataContext.RunComponentStatus
                                   .Where(x => x.RunNumberId == runId)
                                   .ToList<RunComponentStatus>();
               if (list == null)
                   throw new Exception("No runcomponentstatus found by runId in database.");
               else
                   return list;
       }

       public IQueryable<RunComponentStatus> FindAll()
       {
            return objDataContext.RunComponentStatus;
       }

       public IQueryable<RunComponentStatus> FindAllActive()
       {
            return objDataContext.RunComponentStatus;
       }

       public RunComponentStatus Find(int id)
       {
                return objDataContext.RunComponentStatus.SingleOrDefault(x => x.RunComponentStatusId == id);
       }
       
       public int Save(RunComponentStatus dao)
       {
            RunComponentStatus entity = objDataContext.RunComponentStatus.SingleOrDefault(x => x.RunComponentStatusId == dao.RunComponentStatusId);
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.RunComponentStatus.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.RunComponentStatusId;
        }

       public bool Delete(int id)
       {
           throw new NotImplementedException();
       }

        public bool UpdateStatus(int id)
        {
            throw new NotImplementedException();
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
        // ~RunComponentStatusRepository() {
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
