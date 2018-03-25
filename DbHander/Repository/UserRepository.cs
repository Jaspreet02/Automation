using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DbHander
{
    public class UserRepository : AbstractRepository, IUserRepository
    {
        public IQueryable<ApplicationUser> FindAll()
        {
            return objDataContext.Users;
        }

        public IQueryable<ApplicationUser> FindAllActive()
        {
            return objDataContext.Users.Where(x => x.Status == true);
        }  

        public List<string> UserIds(string userId)
        {
            SqlParameter param1 = new SqlParameter("@userId", userId);
            var result = objDataContext.Database.SqlQuery<string>("EXEC GetUserList @userId", param1).ToList();
            return result;
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
        // ~UserRepository() {
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
