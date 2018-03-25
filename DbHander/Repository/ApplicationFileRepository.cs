using System;
using System.Linq;

namespace DbHander
{
    public class ApplicationFileRepository : AbstractRepository, IApplicationFilesRepository
    {
        public IQueryable<ApplicationFile> FindAll()
        {
              var  list = objDataContext.ApplicationFiles
                                    .Where(x => (x.Status));
               return list;
         }

        public IQueryable<ApplicationFile> FindAllActive()
        {
                return objDataContext.ApplicationFiles.Where(x => x.Status == true);
         }

        public ApplicationFile Find(int id)
        {
                return objDataContext.ApplicationFiles.SingleOrDefault(x => x.ApplicationFileId.Equals(id));
          }

        public int Save(ApplicationFile dao)
        {  
                ApplicationFile entity = objDataContext.ApplicationFiles.SingleOrDefault(x => x.ApplicationFileId.Equals(dao.ApplicationFileId));
                if (entity != null)
                {
                    dao.ModifiedAt = DateTimeOffset.Now;
                    objDataContext.Entry(entity).CurrentValues.SetValues(dao);
                }
                else
                {
                    dao.CreatedAt = DateTimeOffset.Now;
                    dao.ModifiedAt = dao.CreatedAt;
                    objDataContext.ApplicationFiles.Add(dao);
                }
                objDataContext.SaveChanges();
           
            return dao.ApplicationFileId;
        }

        public bool Delete(int id)
        {

                ApplicationFile dbEntity = objDataContext.ApplicationFiles.Where(x => x.ApplicationFileId == id).FirstOrDefault();
                if (dbEntity != null)
                {
                    objDataContext.ApplicationFiles.Remove(dbEntity);
                    objDataContext.SaveChanges();
                }
                return true;
        }

        public IQueryable<ApplicationFile> GetApplicationFileListByAppID(int AppId)
        {
            return objDataContext.ApplicationFiles.Where(x => x.ApplicationId == AppId);
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
        // ~ApplicationFileRepository() {
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
