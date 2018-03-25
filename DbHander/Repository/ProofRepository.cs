using System;
using System.Linq;

namespace DbHander
{
    public class ProofRepository : AbstractRepository, IProofRepository
    {
        public IQueryable<Proof> FindAll()
        {
            var proofs = objDataContext.Proofs;
            if (proofs == null)
                return null;
            else
                return proofs;
        }

        public IQueryable<Proof> FindAllActive()
        {
            return objDataContext.Proofs.Where(x => x.Status == true);
        }

        public Proof Find(int id)
        {
            return objDataContext.Proofs.SingleOrDefault(x => x.ProofId.Equals(id));
        }

        public int Save(Proof dao)
        {
            Proof entity = objDataContext.Proofs.SingleOrDefault(x => x.ProofId.Equals(dao.ProofId));
            if (entity != null)
            {
                entity.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                objDataContext.Proofs.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ProofId;
        }

        public bool Delete(int id)
        {
            var objProofFiles = objDataContext.ProofFiles.Where(x => x.ProofId == id);

            if (objProofFiles != null)
            {
                foreach (var item in objProofFiles)
                {
                    objDataContext.ProofFiles.Remove(item);
                }
            }

            Proof dbEntity = objDataContext.Proofs.Where(x => x.ProofId == id).FirstOrDefault();
            if (dbEntity != null)
            {
                objDataContext.Proofs.Remove(dbEntity);
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
        // ~ProofRepository() {
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
