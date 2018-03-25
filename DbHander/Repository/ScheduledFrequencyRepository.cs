using System;
using System.Linq;

namespace DbHander
{
    public class ScheduledFrequencyRepository : AbstractRepository, IScheduledFrequencyRepository
    {        
        public IQueryable<ScheduledFrequency> FindAll()
        {

            return objDataContext.ScheduledFrequencies;
        }

        public IQueryable<ScheduledFrequency> FindAllActive()
        {
            return objDataContext.ScheduledFrequencies.Where(x => x.Status);
        }

        public ScheduledFrequency Find(int id)
        {
            return objDataContext.ScheduledFrequencies.SingleOrDefault(x => x.ScheduledFrequencyId.Equals(id));
        }

        public int Save(ScheduledFrequency dao)
        {
            ScheduledFrequency entity = objDataContext.ScheduledFrequencies.SingleOrDefault(x => x.ScheduledFrequencyId.Equals(dao.ScheduledFrequencyId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            { 
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.ScheduledFrequencies.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ScheduledFrequencyId;
        }

        public bool Delete(int id)
        {
                ScheduledFrequency dbEntity = objDataContext.ScheduledFrequencies.Where(x => x.ScheduledFrequencyId == id).Select(x => x).SingleOrDefault();
               
                objDataContext.ScheduledFrequencies.Remove(dbEntity);
                objDataContext.SaveChanges();
                return true;
        }

        public IQueryable<ScheduledFrequency> GetScheduledFrequencyListbyAppId(int appId)
        {
                return objDataContext.ScheduledFrequencies.Where(x => x.ApplicationId.Equals(appId));
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
        // ~ScheduledFrequencyRepository() {
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
