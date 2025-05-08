using RFIDDAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFIDDAL.Models;

namespace RFIDDAL.Repositories.Repositories
{
    public class RoleRepository : RepositoryBase<AspNetRole>, IRoleRepository
    {
        public RoleRepository(RFIDdbContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
