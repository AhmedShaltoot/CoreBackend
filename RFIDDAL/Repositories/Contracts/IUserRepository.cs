using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static RFIDDAL.Repositories.Repositories.UserRepository;

namespace RFIDDAL.Repositories.Contracts
{
    public interface IUserRepository : IRepositoryBase<AspNetUser>
    {
        PagedResult<UserWithRole> FindUsersWithRolesByConditionPager(int page, int pageSize, Expression<Func<AspNetUser, bool>> expression);
    }
}
