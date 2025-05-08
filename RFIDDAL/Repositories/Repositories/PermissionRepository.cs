using Microsoft.EntityFrameworkCore;
using RFIDDAL.Models;
using RFIDDAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RFIDDAL.Repositories.Repositories
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(RFIDdbContext repositoryContext) : base(repositoryContext)
        {
        }
        
    }
}
