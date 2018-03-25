using System.Collections.Generic;
using System.Linq;

namespace DbHander
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> FindAll();
        IQueryable<ApplicationUser> FindAllActive();
        List<string> UserIds(string userId);
    }
}
