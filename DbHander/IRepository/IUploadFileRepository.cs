using System.Collections.Generic;

namespace DbHander
{
   public interface IUploadFileRepository : IGenericRepository<UploadFile>
    {
        IEnumerable<UploadFile> FindAllByAppNComponentId(int appId, int componentId);
    }
}
