using System;
using System.Linq;

namespace DbHander
{
    public class ProcComponentRepository : AbstractRepository, IProcComponantRepository
    {        
        public IQueryable<ProcComponent> FindAll()
        {
                var result = objDataContext.ProcComponents;
                return result;
           }

        public IQueryable<ProcComponent> FindAllActive()
        {
                var result = objDataContext.ProcComponents.Where(x => x.Status == true);
                return result;
         }

        public ProcComponent Find(int id)
        {
            return objDataContext.ProcComponents.SingleOrDefault(x => x.ProcComponentId.Equals(id));
        }

        public int Save(ProcComponent dao)
        {
            ProcComponent entity = objDataContext.ProcComponents.SingleOrDefault(x => x.ProcComponentId.Equals(dao.ProcComponentId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.ProcComponents.Add(dao);
            }
            objDataContext.SaveChanges();
            return   dao.ProcComponentId;
        }

        public bool Delete(int id)
        {
                var procComponentObj = objDataContext.ProcComponents.Where(x => x.ProcComponentId == id).SingleOrDefault();
                if (procComponentObj != null)
                    objDataContext.ProcComponents.Remove(procComponentObj);
                objDataContext.SaveChanges();
                return true;
       }
              
        public bool UpdateStatus(int id)
        {
            throw new NotImplementedException();
        }

        public ProcComponent GetFirstRecord()
        {
                return objDataContext.ProcComponents.FirstOrDefault();
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
        // ~ProcRepository() {
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

