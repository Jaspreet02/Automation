using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public class AppCommandArgumentRepository : AbstractRepository, IAppCommandArgumentRepository
    {
        /// <summary>
        /// Find all CommandArguments
        /// </summary>
        /// <returns>Return List of CommandArguments</returns>
        public IQueryable<AppCommandArgument> FindAll()
        {
            var result = objDataContext.AppCommandArguments;
            return result;
        }
        /// <summary>
        /// Find all CommandArguments whose status is active
        /// </summary>
        /// <returns>Return List of CommandArguments</returns>
        public IQueryable<AppCommandArgument> FindAllActive()
        {
            var result = objDataContext.AppCommandArguments.Where(x => x.Status == true);
            return result;
        }
        /// <summary>
        /// Find CommandArgument by id
        /// </summary>
        /// <param name="id">CommandArgument id</param>
        /// <returns>Return CommandArgument</returns>
        public AppCommandArgument Find(int id)
        {
            var result = objDataContext.AppCommandArguments.SingleOrDefault(x => x.AppCommandArgumentId.Equals(id));
            return result;
        }
        /// <summary>
        /// Save CommandArgument
        /// </summary>
        /// <param name="dao">CommandArgument</param>
        /// <returns>Return true if saved successfull otherwise false</returns>
        public int Save(AppCommandArgument dao)
        {
            AppCommandArgument entity = objDataContext.AppCommandArguments.SingleOrDefault(x => x.AppCommandArgumentId.Equals(dao.AppCommandArgumentId));
            if (entity != null)
            {
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                objDataContext.AppCommandArguments.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.AppCommandArgumentId;
        }

        /// <summary>
        /// Delete CommandArgument by id
        /// </summary>
        /// <param name="id">Id of CommandArgument to be deleted</param>
        /// <returns>Return true if deleted successfully,otherwise retun false</returns>
        public bool Delete(int id)
        {
            AppCommandArgument dbEntity = objDataContext.AppCommandArguments.Where(x => x.AppCommandArgumentId == id).FirstOrDefault();
            if (dbEntity != null)
            {
                objDataContext.AppCommandArguments.Remove(dbEntity);
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
        // ~AppCommandArgumentRepository() {
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
