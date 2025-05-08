using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using RFIDBLL.AutoMapperConfig;
using RFIDBLL.DTOs;
using RFIDBLL.HelperClasses;
using RFIDBLL.Services.Contracts;
using RFIDDAL.Models;
using RFIDDAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Services
{
    public class RoleService : IRoleService
    {
        protected IRepositoryWrapper _repoWrapper;
        public RoleService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public object GetAllRolesPager(bool isActive, int pageSize, int currentPage, string keyword = "")
        {
            var res = _repoWrapper.Role.FindByConditionPager(currentPage, pageSize, m => (string.IsNullOrEmpty(keyword) || m.Name.ToLower().Contains(keyword.ToLower())));
            return res;
        }
        public object GetRolesDDL()
        {
            return _repoWrapper.Role.FindAll().Select(m => new
            {
                m.Id,
                m.Name
            }).ToList();
        }

        public object GetRoleById(string roleId)
        {
            return _repoWrapper.Role.FindByCondition(m => m.Id == roleId).FirstOrDefault();
        }
        public bool UpdateRole(RoleDTO role)
        {
            try
            {
                var roleDB = _repoWrapper.Role.FindByCondition(u=> u.Id == role.RoleId).FirstOrDefault();
                roleDB.Name = role.Name;
                _repoWrapper.Role.Update(roleDB);
                _repoWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddRole(RoleDTO role)
        {
            try
            {
                var newrole = new AspNetRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = role.Name
                };
                _repoWrapper.Role.Create(newrole);
                _repoWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string GenerateSecurityStamp()
        {
            byte[] randomBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        public bool DeleteRole(string roleId)
        {
            try
            {
                var roleRoles = _repoWrapper.UserRole.FindByCondition(u=> u.RoleId == roleId).Any();
                if(roleRoles == true)
                {
                    return false;
                }
                var roleDB = _repoWrapper.Role.FindByCondition(u=> u.Id == roleId).FirstOrDefault();
                _repoWrapper.Role.Delete(roleDB);
                _repoWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string decodebase(string baseuri)
        {
            // Remove any whitespace or invalid characters
            string base64Input = baseuri.Trim();

            // Ensure the string has the correct length for base64 encoding
            //if (base64Input.Length % 4 != 0)
            //{
            //    // Adjust padding if necessary by adding '=' characters
            //    base64Input = base64Input.PadRight(base64Input.Length + (4 - base64Input.Length % 4), '=');
            //}

            try
            {
                // Decode the base64 string to bytes
                byte[] decodedBytes = Convert.FromBase64String(base64Input);
                // Convert the bytes to a UTF-8 string
                return Encoding.UTF8.GetString(decodedBytes);
            }
            catch (Exception ex)
            {
                return "Error: The input is not a valid Base-64 string.";
            }
        }
        public async Task<bool> AssignRoleToUser(string userId, string roleId)
        {
            var userRole = _repoWrapper.UserRole.FindByCondition(c => c.UserId == userId).FirstOrDefault();
            if (userRole != null && userRole.RoleId != roleId)
            {
                userRole.RoleId = roleId;
                _repoWrapper.UserRole.Update(userRole);
                _repoWrapper.Save();
            }
            else if (roleId == null)
            {
                AspNetUserRole aspNetUserRole = new AspNetUserRole
                {
                    RoleId = roleId,
                    UserId = userId
                };
                _repoWrapper.UserRole.Create(aspNetUserRole);
                _repoWrapper.Save();
            }
            return true;
        }
        public async Task<bool> AssignPermissionsToRole(string roleId, List<int> permissionIds)
        {
            var oldPermissions = _repoWrapper.RolePermission.FindByCondition(u => u.RoleId == roleId).ToList();
            if (oldPermissions != null && oldPermissions.Count > 0)
            {
                _repoWrapper.RolePermission.DeleteBulk(oldPermissions);
                _repoWrapper.Save();
            }
            List<RolePermission> rolePermissions = permissionIds
        .Select(permissionId => new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        })
        .ToList();

            _repoWrapper.RolePermission.CreateBulk(rolePermissions);
            _repoWrapper.Save();

            return true;
        }
        public async Task<List<string>> GetUserPermissions(string userId)
        {
            //var userRoleId = _repoWrapper.AspNetUserRole.FindByCondition(c => c.UserId == userId).Select(c => c.RoleId).FirstOrDefault();

            //if (userRoleId != null)
            //{
            var permissions = _repoWrapper.RolePermission.FindByCondition(c => c.RoleId == userId).Include(c => c.Permission).Select(c => c.Permission.PermissionName + c.Permission.PageName).ToList();
            return permissions;
            // }
        }
        public async Task<object> GetRolePermissions(string roleId)
        {
            try
            {
                //var permissions = _repoWrapper.RolePermission.FindByCondition(c => c.RoleId == roleId).Include(c => c.Permission).Select(c => c.Permission.PermissionName).ToList();
                var permissions = _repoWrapper.RolePermission.FindByCondition(c => c.RoleId == roleId).Select(c => new { PermissionId = c.PermissionId }).ToList();
                return permissions;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<IEnumerable<IGrouping<string, Permission>>> GetPermissions()
        {
            try
            {
                var permissions = _repoWrapper.Permission
             .FindByCondition(c => c.IsActive == true)
             .AsEnumerable()
             .GroupBy(c => c.PageNameAr);
                return permissions.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public object GetRoplesDDL()
        {
            throw new NotImplementedException();
        }
    }
}
