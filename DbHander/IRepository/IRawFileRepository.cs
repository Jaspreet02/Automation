using System;
using System.Collections.Generic;

namespace DbHander
{
    public interface IRawFileRepository : IGenericRepository<RawFile>
    {
        List<RawFile> GetRawFileListByRunNumberId(Int32 RunNumberId);    
    }
}
