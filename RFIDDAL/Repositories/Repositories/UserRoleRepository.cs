using RFIDDAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFIDDAL.Models;

namespace RFIDDAL.Repositories.Repositories
{
    public class UserRoleRepository : RepositoryBase<AspNetUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(RFIDdbContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
