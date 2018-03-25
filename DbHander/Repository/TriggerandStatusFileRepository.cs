using System;
using System.Linq;


namespace DbHander
{
    public class TriggerandStatusFilesRepository : AbstractRepository,ITriggerandStatusFileRepository
    {

        public IQueryable<TriggerandStatusFile> FindAll()
        {
            return objDataContext.TriggerandStatusFiles;
        }

        public TriggerandStatusFile FindbyComponentId(int componentId)
        {
            return objDataContext.TriggerandStatusFiles.FirstOrDefault(x => x.ComponentId.Equals(componentId));
        }

        public IQueryable<TriggerandStatusFile> FindAllActive()
        {
            return objDataContext.TriggerandStatusFiles.Where(x => x.Status);
        }

        public TriggerandStatusFile Find(int id)
        {
            return objDataContext.TriggerandStatusFiles.SingleOrDefault(x => x.ComponentId.Equals(id));
        }

          public int Save(TriggerandStatusFile dao)
        {

            TriggerandStatusFile entity = objDataContext.TriggerandStatusFiles.SingleOrDefault(x => x.ComponentId.Equals(dao.ComponentId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.TriggerandStatusFiles.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ComponentId;
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
        // ~ComponentTriggerandStatusFilesRepository() {
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
