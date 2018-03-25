using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace DbHander
{
    public class ApplicationRepository : AbstractRepository, IApplicationRepository
    {
        public Dictionary<int, List<int>> ApplicationIdsbyClients(List<int> clientIds)
        {
            var result =  objDataContext.Applications.Where(x => clientIds.Contains(x.ClientId) && x.Status == true).GroupBy(x=> x.ClientId).Select(x=> new { Id = x.Key, List = x.ToList().Select(A => A.ApplicationId) }).ToDictionary(t=> t.Id, t=> t.List.ToList());
            return result;
        }

        public IQueryable<Application> FindAll()
        {
                return objDataContext.Applications;
        }

        public IQueryable<Application> FindAllActive()
        {
                var result = objDataContext.Applications.Where(x => x.Status == true);
                return result;
        }

        public Application Find(int id)
        {
                return objDataContext.Applications.SingleOrDefault(x => x.ApplicationId.Equals(id));
        }

        public int Save(Application dao)
        {
                Application entity = objDataContext.Applications.SingleOrDefault(x => x.ApplicationId.Equals(dao.ApplicationId));
                if (entity != null)
                {
                    dao.ModifiedAt = DateTimeOffset.Now;
                    objDataContext.Entry(entity).CurrentValues.SetValues(dao);
                }
                else
                {
                    dao.CreatedAt = DateTimeOffset.Now;
                    dao.ModifiedAt = dao.CreatedAt;
                    objDataContext.Applications.Add(dao);
                }
                objDataContext.SaveChanges();
                //using (var connect = new DataContext())
                //{
                //    connect.Applications.Attach(dao);
                //    if (dao.ApplicationId > 0)connect.Entry(dao).State = System.Data.Entity.EntityState.Modified;
                //    else connect.Entry(dao).State = System.Data.Entity.EntityState.Added;
                //    connect.SaveChanges();
                //}
           
            return dao.ApplicationId;
        }

        public bool Delete(int id)
        {
                var objAppComp = objDataContext.ApplicationComponents.Where(x => x.ApplicationId == id);
                var compId = objAppComp.Select(x => x.ApplicationComponentId).ToList();
                if (compId != null)
                {
                    foreach (var item in compId)
                    {
                        var objConfigFiles = objDataContext.ComponentConfigs.Where(x => x.ApplicationComponentId == item);

                        if (objConfigFiles != null)
                        {
                            foreach (var file in objConfigFiles)
                            {
                                objDataContext.ComponentConfigs.Remove(file);
                            }
                        }
                    }
                }
                if (objAppComp != null)
                {
                    foreach (var item in objAppComp)
                    {
                        objDataContext.ApplicationComponents.Remove(item);
                    }
                }

                Application dbEntity = objDataContext.Applications.Where(x => x.ApplicationId == id).Select(x => x).SingleOrDefault();
                objDataContext.Applications.Remove(dbEntity);
                objDataContext.SaveChanges();
           
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


        //#region CloneApplication
        ///// <summary>
        ///// Show Info of Application to be copied
        ///// </summary>
        ///// <param name="AppID">Application ID of app to whome we are copying</param>
        ///// <returns>Return Application name,App code,Number of app component,Count of appfiles</returns>
        //public AppCloneDetail GetCloneAppInfo(int AppID)
        //{
        //    try
        //    {
        //        var appInfo = DbHander.Applications.Where(x => x.ID == AppID).Select(x => new AppCloneDetail() { AppCode = x.Code, AppName = x.ApplicationName }).FirstOrDefault();
        //        var appfiles = DbHander.ApplicationFiles.Where(x => x.AppId == AppID).Count();
        //        var appconponent = DbHander.ApplicationComponents.Where(x => x.ApplicationsId == AppID).Count();
        //        appInfo.AppComponent = appconponent;
        //        appInfo.AppFiles = appfiles;
        //        return appInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int CloneApplication(int id, string name, string code, string mask)
        //{
        //    using (SqlConnection connection = new SqlConnection(ConnectionString))
        //    {
        //        connection.Open();
        //        using (var cmdProc = new SqlCommand("uspCloneApplication", connection))
        //        {
        //            cmdProc.CommandTimeout = 500;
        //            cmdProc.Parameters.Clear();
        //            cmdProc.CommandType = CommandType.StoredProcedure;
        //            SqlParameter parmApplicationName = new SqlParameter("@ApplicationName", SqlDbType.VarChar);
        //            parmApplicationName.Value = name;
        //            parmApplicationName.SqlDbType = SqlDbType.VarChar;
        //            parmApplicationName.Size = 50;
        //            parmApplicationName.Direction = ParameterDirection.Input;
        //            cmdProc.Parameters.Add(parmApplicationName);

        //            SqlParameter parmCode = new SqlParameter("@Code", SqlDbType.VarChar);
        //            parmCode.Value = code;
        //            parmCode.SqlDbType = SqlDbType.VarChar;
        //            parmCode.Size = 50;
        //            parmCode.Direction = ParameterDirection.Input;
        //            cmdProc.Parameters.Add(parmCode);

        //            SqlParameter parmMask = new SqlParameter("@Mask", SqlDbType.VarChar);
        //            parmMask.Value = mask;
        //            parmMask.SqlDbType = SqlDbType.VarChar;
        //            parmMask.Size = 50;
        //            parmMask.Direction = ParameterDirection.Input;
        //            cmdProc.Parameters.Add(parmMask);

        //            SqlParameter parmID = new SqlParameter("@ID", SqlDbType.Int);
        //            parmID.Value = id;
        //            parmID.SqlDbType = SqlDbType.Int;
        //            parmID.Direction = ParameterDirection.Input;
        //            cmdProc.Parameters.Add(parmID);

        //            //SqlParameter parmNewappId = new SqlParameter("@NewAppId", SqlDbType.Int);
        //            //parmNewappId.Value = 0;
        //            //parmNewappId.SqlDbType = SqlDbType.Int;
        //            //parmNewappId.Direction = ParameterDirection.Output;
        //            //cmdProc.Parameters.Add(parmNewappId);

        //            var newAppId = Convert.ToInt32(cmdProc.ExecuteScalar());

        //            // var fileSplittedPath = string.Empty;
        //            //var splittedPath = cmdProc.Parameters["@FilesSplittedPath"].Value; //This will be path
        //            return newAppId;
        //        }
        //    }
        //}

        //public bool Save(Application dao, out int id)
        //{
        //    throw new NotImplementedException();
        //}

        //IQueryable<Application> IGenericRepository<Application>.FindAllActive()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool UpdateStatus(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion


    }

}
