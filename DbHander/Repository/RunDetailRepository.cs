using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace DbHander
{
    public class RunDetailsRepository : AbstractRepository, IRunDetailsRepository
    {
        /// <summary>
        /// gets the Application Id bu Run Number.
        /// </summary>
        /// <param name="runNumber">RunNumber</param>
        /// <returns></returns>
        public int GetApplicationIdByRunNumber(String runNumber)
        {
            return objDataContext.RunDetails.Where(x => x.RunNumber.Trim().ToUpper().Equals(runNumber.Trim().ToUpper())).Select(x => x.ApplicationId).First();
        }

        /// <summary>
        ///returns the Run Number id of given RunNumber
        /// </summary>
        /// <param name="runNumber">Run Number</param>
        /// <returns>Run Number Id</returns>
        public int GetRunNumberIdByRunNumber(string runNumber)
        {
            return objDataContext.RunDetails.Where(x => x.RunNumber == runNumber).Select(x => x.RunDetailId).FirstOrDefault();
        }

        /// <summary>
        /// returns the Run Detail  of a Run Number
        /// </summary>
        /// <param name="runNumber"></param>
        /// <returns></returns>
        public RunDetail GetRunDetailByRunNumber(string runNumber)
        {
            return objDataContext.RunDetails.SingleOrDefault(x => x.RunNumber == runNumber && x.Status == true);
        }

        /// <summary>
        /// update the Run by RunNumberId
        /// </summary>
        /// <param name="runNumberId"></param>
        /// <param name="UpdatedValue"></param>
        public bool UpdateRunStatusByRunNumberId(int runNumberId, byte UpdatedValue)
        {
            var result = objDataContext.RunDetails.FirstOrDefault(x => x.RunDetailId == runNumberId);
            var Count = objDataContext.RunComponentStatus.Where(x => x.RunNumberId == runNumberId).Select(x => x.ComponentStatusId).Distinct().ToList();
            result.RunNumberStatusId = UpdatedValue;
            if (Count.Count == 1)
            {
                if (Count.First() == (int)ComponentStatusType.Completed)
                {
                    result.RunNumberStatusId = (int)RunNumberStatusType.Completed;
                }
            }
            objDataContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// gets the AppComponentType By RunNumber and ComponentId
        /// </summary>
        /// <param name="runNumber"></param>
        /// <param name="componentId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>  
        public bool GetAppComponentTypeByRunCompStatustId(int RuncompStatusId, int appId)
        {
            var IcomponentId = objDataContext.RunComponentStatus.Where(x => x.RunComponentStatusId == RuncompStatusId).SingleOrDefault().ComponentId;
            var objAppComponent = (from appComponent in objDataContext.ApplicationComponents
                                   where appComponent.ApplicationId == appId && appComponent.ComponentId == IcomponentId
                                   select appComponent).FirstOrDefault();
            return objAppComponent.IsOptional;
        }

        public IQueryable<RunDetail> FindAll()
        {
            return objDataContext.RunDetails;
        }

        public IQueryable<RunDetail> FindAllActive()
        {
            return objDataContext.RunDetails.Include("RunComponentStatus")
                                   .Where(x => x.Status == true);
        }

        public RunDetail Find(int id)
        {
            return objDataContext.RunDetails.SingleOrDefault(x => x.RunDetailId == id);
        }

        public int Save(RunDetail dao)
        {
            RunDetail entity = objDataContext.RunDetails.SingleOrDefault(x => x.RunDetailId.Equals(dao.RunDetailId));
            if (entity != null)
            {
                dao.ModifiedAt = DateTimeOffset.Now;
                objDataContext.Entry(entity).CurrentValues.SetValues(dao);
            }
            else
            {
                dao.CreatedAt = DateTimeOffset.Now;
                dao.ModifiedAt = dao.CreatedAt;
                objDataContext.RunDetails.Add(dao);
            }
            objDataContext.SaveChanges();
            return dao.RunDetailId;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<RunDetail> GetRunDetailByStatus(int status)
        {
            return objDataContext.RunDetails.Where(x => x.RunNumberStatusId == status && x.Status);
        }

        public string GetLastRunNumber(int appId)
        {
            string runNumber = "000000";
            lock (objDataContext.RunDetails)
            {
                if (objDataContext.RunDetails.Count() > 0)
                {

                    runNumber = (from runDetail in objDataContext.RunDetails
                                 where runDetail.ApplicationId.Equals(appId)
                                 orderby runDetail.CreatedAt descending
                                 select runDetail.RunNumber).FirstOrDefault();
                }
            }
            return runNumber;
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
        // ~RunDetailsRepository() {
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
