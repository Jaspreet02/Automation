using System;
using System.Linq;

namespace DbHander
{
    public class ProofFilesRepository : AbstractRepository, IProofFilesRepository
    {
        public IQueryable<ProofFile> FindAll()
        {
                var proofFile = objDataContext.ProofFiles;
                return proofFile;
         }

        public IQueryable<ProofFile> FindAllActive()
        {
            return objDataContext.ProofFiles;
        }

        public ProofFile Find(int id)
        {
            return objDataContext.ProofFiles.SingleOrDefault(x => x.ProofFileId.Equals(id));
        }

        public int Save(ProofFile dao)
        {
            ProofFile entity = objDataContext.ProofFiles.SingleOrDefault(x => x.ProofFileId.Equals(dao.ProofFileId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.ProofFiles.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ProofFileId;
        }
            

        public bool Delete(int id)
        {
                ProofFile dbEntity = objDataContext.ProofFiles.Where(x => x.ProofFileId == id).FirstOrDefault();
                if (dbEntity != null)
                {
                    objDataContext.ProofFiles.Remove(dbEntity);
                    objDataContext.SaveChanges();
                }
                return true;
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
        // ~ProofFilesRepository() {
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
