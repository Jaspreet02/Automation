using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHander
{
  public abstract class AbstractRepository
    {
       internal readonly DataContext objDataContext;

       internal AbstractRepository()
       {
           objDataContext = new DataContext();
       }
    }
}
