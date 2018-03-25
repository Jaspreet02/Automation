using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbHander.IRepository;
using DbHander.DomainClass;
using BLW.DAL.Exceptions;
using System.Data.SqlClient;
using BLW.DAL.Extensions;

namespace BLW.DAL.Repository
{
    public class GmcSchenerioRepository : AbstractRepository,IGmcSchenerioRepository
    {
        public IEnumerable<GmcSchenerioDao> FindAll()
        {
            List<GmcSchenerioDao> list = new List<GmcSchenerioDao>();
            try
            {
                list = DbHander.GmcAddOnsSchenerios
                                    .Select(x => Mapper.Mapper.Map(x)).ToList<GmcSchenerioDao>();
               
                    return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<GmcSchenerioDao> FindAllActive()
        {
            return FindAll();
        }

        public GmcSchenerioDao Find(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save(GmcSchenerioDao dao)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
        public bool ChangeStatus(int id, bool status)
        {
            throw new NotImplementedException();
        }
    }
}
