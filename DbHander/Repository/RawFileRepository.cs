using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public class RawFileRepository : AbstractRepository, IRawFileRepository
    {
        public IQueryable<RawFile> FindAll()
        {
            return objDataContext.RawFiles;
        }

        public IQueryable<RawFile> FindAllActive()
        {
            return objDataContext.RawFiles.Where(x => x.Status);
        }

        public RawFile Find(int id)
        {
            return objDataContext.RawFiles.SingleOrDefault(x => x.RawFileId.Equals(id));
        }

        public int Save(RawFile dao)
        {

            RawFile entity = objDataContext.RawFiles.SingleOrDefault(x => x.RawFileId.Equals(dao.RawFileId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.RawFiles.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.RawFileId;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<RawFile> GetRawFileListByRunNumberId(int RunNumberId)
        {
                return FindAll().Where(x => x.RunNumberId == RunNumberId).ToList();
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
        // ~RawFileRepository() {
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
