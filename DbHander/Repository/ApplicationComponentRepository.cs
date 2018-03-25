using System;
using System.Linq;


namespace DbHander
{
    public class ApplicationComponentRepository : AbstractRepository, IApplicationComponentRepository
    {
        /// <summary>
        /// Find all Application Components
        /// </summary>
        /// <returns>Return List of Application Components</returns>
        public IQueryable<ApplicationComponent> FindAll()
        {
            return objDataContext.ApplicationComponents.OrderByDescending(x=> x.CreatedAt);
        }
        /// <summary>
        /// Find all Active Application Components
        /// </summary>
        /// <returns>Return List of Application Components</returns>
        public IQueryable<ApplicationComponent> FindAllActive()
        {
            return objDataContext.ApplicationComponents.Where(x => x.Status == true);
        }
        /// <summary>
        /// Find Application Component by id
        /// </summary>
        /// <param name="id">Application Component id</param>
        /// <returns>Application Component</returns>
        public ApplicationComponent Find(int id)
        {
            return objDataContext.ApplicationComponents.SingleOrDefault(x => x.ApplicationComponentId == id);
        }

        /// <summary>
        /// Save ApplicationComponent
        /// </summary>
        /// <param name="dao">ApplicationComponent</param>
        /// <returns>Return true of saved successfully,otherwise false</returns>
        public int Save(ApplicationComponent dao)
        {
            ApplicationComponent entity = objDataContext.ApplicationComponents.SingleOrDefault(x => x.ApplicationComponentId == dao.ApplicationComponentId);
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.ApplicationComponents.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.ApplicationComponentId;
        }

        /// <summary>
        /// Delete ApplicationComponent by ApplicationComponentId
        /// </summary>
        /// <param name="id">ApplicationComponentId</param>
        /// <returns>Return true if deleted successfully,otherwise return false</returns>
        public bool Delete(int id)
        {
            var entity = objDataContext.ApplicationComponents.SingleOrDefault(x => x.ApplicationComponentId == id);
            if (entity != null)
            {
                objDataContext.ApplicationComponents.Remove(entity);
                objDataContext.SaveChanges();
            }
            return true;
        }
        
        public IQueryable<ApplicationComponent> GetApplicationComponentListbyappId(int applicationId)
        {
            return objDataContext.ApplicationComponents.Where(x => x.ApplicationId.Equals(applicationId)).OrderBy(x => x.ComponentOrder);
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
        // ~ApplicationComponentRepository() {
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
