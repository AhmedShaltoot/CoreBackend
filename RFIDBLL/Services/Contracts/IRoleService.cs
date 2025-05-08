using RFIDBLL.DTOs;
using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Contracts
{
    public interface IRoleService
    {
        object GetAllRolesPager(bool isActive, int pageSize, int currentPage, string keyword = "");
        object GetRoleById(string RoleId);
        bool AddRole(RoleDTO Role);
        bool UpdateRole(RoleDTO role);
        bool DeleteRole(string RoleId);
        object GetRolesDDL();
        string decodebase(string baseuri);
        Task<bool> AssignRoleToUser(string userId, string roleId);
        Task<bool> AssignPermissionsToRole(string roleId, List<int> permissionIds);
        Task<List<string>> GetUserPermissions(string userId);
        Task<object> GetRolePermissions(string roleId);
        Task<IEnumerable<IGrouping<string, Permission>>> GetPermissions();
    }
}
