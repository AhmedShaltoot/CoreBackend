using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Contracts
{
    public interface IPermissionService
    {
        object GetAllPermissionPager(bool isActive, int pageSize, int currentPage, string keyword = "");
        object GetPermissionDDL();
        Permission GetPermissionById(int id);
        bool AddPermission(Permission entity);
        bool UpdatePermission(Permission entity);
        bool DeletePermission(int id);
    }
}
