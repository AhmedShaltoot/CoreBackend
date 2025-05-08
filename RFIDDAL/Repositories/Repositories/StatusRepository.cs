using RFIDDAL.Models;
using RFIDDAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RFIDDAL.Repositories.Repositories
{
    public class StatusRepository : RepositoryBase<Status>, IStatusRepository
    {
        public StatusRepository(RFIDdbContext repositoryContext) : base(repositoryContext)
        {
        }
        
    }
}
