using System.Linq;

namespace DbHander
{
   public  interface IApplicationFilesRepository : IGenericRepository<ApplicationFile>
    {
       IQueryable<ApplicationFile> GetApplicationFileListByAppID(int AppId);
    }
}
