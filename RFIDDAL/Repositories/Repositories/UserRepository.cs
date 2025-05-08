using RFIDDAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFIDDAL.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace RFIDDAL.Repositories.Repositories
{
    public class UserRepository : RepositoryBase<AspNetUser>, IUserRepository
    {
        public UserRepository(RFIDdbContext repositoryContext) : base(repositoryContext)
        {

        }
        public PagedResult<UserWithRole> FindUsersWithRolesByConditionPager(int page, int pageSize, Expression<Func<AspNetUser, bool>> expression)
        {
            var result = new PagedResult<UserWithRole>();

            var userQuery = RepositoryContext.Set<AspNetUser>().Where(expression);

            // Join with AspNetUserRoles to get roles for users
            var userRolesQuery = from user in userQuery
                                 join userRole in RepositoryContext.Set<AspNetUserRole>() on user.Id equals userRole.UserId
                                 join role in RepositoryContext.Set<AspNetRole>() on userRole.RoleId equals role.Id
                                 select new UserWithRole
                                 {
                                     User = user,
                                     RoleName = role.Name,
                                     RoleId = role.Id
                                 };

            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = userRolesQuery.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = userRolesQuery.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public class UserWithRole
        {
            public AspNetUser User { get; set; }
            public string RoleName { get; set; }
            public string RoleId { get; set; }
        }
    }
}
