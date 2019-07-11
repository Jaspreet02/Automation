using DbHander;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
   public class UploadFileRepository : AbstractRepository,IUploadFileRepository
    {
        public bool Delete(int id)
        {
            var objUploadFile = objDataContext.UploadFiles.SingleOrDefault(x => x.UploadFileId == id);
            if (objUploadFile != null)
            {
                objDataContext.UploadFiles.Remove(objUploadFile);
                objDataContext.SaveChanges();
            }
            return true;
        }

        public UploadFile Find(int id)
        {
            return objDataContext.UploadFiles.SingleOrDefault(x => x.UploadFileId == id);
        }

        public IQueryable<UploadFile> FindAll()
        {
                return objDataContext.UploadFiles.OrderByDescending(x=> x.CreatedAt);
           }

        public IQueryable<UploadFile> FindAllActive()
        {
            return objDataContext.UploadFiles.Where(x => x.Status);
        }

        public IEnumerable<UploadFile> FindAllByAppNComponentId(int appId, int componentId)
        {
            return objDataContext.UploadFiles.Where(x => x.ApplicationId == appId && x.ComponentId == componentId && x.Status);
        }
        
        public int Save(UploadFile dao)
        {
                UploadFile entity = objDataContext.UploadFiles.SingleOrDefault(x => x.UploadFileId.Equals(dao.UploadFileId));
                if (entity != null)
                {
                    dao.ModifiedAt = DateTimeOffset.Now;
                    objDataContext.Entry(entity).CurrentValues.SetValues(dao);
                }
                else
                {
                    dao.CreatedAt = DateTimeOffset.Now;
                    dao.ModifiedAt = dao.CreatedAt;
                    objDataContext.UploadFiles.Add(dao);
                }
                objDataContext.SaveChanges();
          
            return dao.UploadFileId;
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
        // ~ApplicationRepository() {
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
