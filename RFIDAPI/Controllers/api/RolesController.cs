using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RFIDAPI.Authentication;
using RFIDBLL.DTOs;
using RFIDBLL.Services.Contracts;

namespace RFIDAPI.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SuperAdmin")]
    //[Authorize(Permissions.Role.View)]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        IRoleService _roleService;


        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,IRoleService roleService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleService = roleService;
        }

        [HttpGet]
        [Route("get-roles-pager")]
        public async Task<dynamic> GetAllRolesPager(bool isActive, int pageSize, int currentPage, string? keyword)
        {
            var roles = _roleService.GetAllRolesPager(isActive, pageSize, currentPage, keyword);
            return roles;
        }
        [HttpGet]
        [Route("get-roles-ddl")]
        public async Task<dynamic> GetAllRolesDDL()
        {
            var roles = _roleService.GetRolesDDL();
            return roles;
        }
        [HttpPut]
        [Route("update-roles")]
        public async Task<dynamic> UpdateRole(RoleDTO role)
        {
            return _roleService.UpdateRole(role);
        }

        [HttpPost]
        [Route("add-role")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RoleDTO model)
        {
            if (!ModelState.IsValid)
                return Ok(await _roleManager.Roles.ToListAsync());

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role already exists!" });
                //return Ok(await _roleManager.Roles.ToListAsync());
            }

            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));

            return Ok(new Response { Status = "Success", Message = "Role created successfully!" });
        }
        [HttpDelete]
        [Route("delete-role")]
        public async Task<bool> DeleteRole(string roleId)
        {
            return _roleService.DeleteRole(roleId);
        }
        [HttpGet]
        [Route("get-user-permissions")]
        public async Task<List<string>> GetUserPermissions(string userId)
        {
            var userPer = _roleService.GetUserPermissions(userId);

            return userPer.Result;
        }
        [HttpGet]
        [Route("get-role-permissions")]
        public async Task<object> GetRolePermissions(string roleId)
        {
            var userPer = _roleService.GetRolePermissions(roleId);

            return userPer.Result;
        }
        [HttpGet]
        [Route("get-all-permissions")]
        public async Task<IEnumerable<PermissionGroupDto>> GetPermissions()
        {
            var userPer = _roleService.GetPermissions();

            return userPer.Result.Select(g => new PermissionGroupDto
            {
                PageName = g.Key,
                Permissions = g.ToList()
            });
        }
        [HttpPost]
        [Route("assign-role-to-user")]
        public async Task<bool> AssignRoleToUser(string userId, string roleId)
        {
            var userPer = _roleService.AssignRoleToUser(userId, roleId);

            return userPer.Result;
        }
        [HttpPut]
        [Route("assign-permissions-to-role")]
        public async Task<bool> AssignPermissionsToRole(SetRolePermissionDTO rolePermissionDTO)
        {
            var userPer = _roleService.AssignPermissionsToRole(rolePermissionDTO.roleId, rolePermissionDTO.permissionIds);

            return userPer.Result;
        }
        [HttpPost]
        [Route("set-role-permissions")]
        //[Authorize(Permissions.Product.View)]
        public async Task<IActionResult> SetRolePermissions(RoleWithClaimsDTO model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
                return NotFound();

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
                await _roleManager.RemoveClaimAsync(role, claim);

            var selectedClaims = model.RoleCalims.Where(c => c.IsSelected).ToList();

            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.DisplayValue));

            return Ok(new Response { Status = "Success", Message = "Role Permissions Updated successfully!" });
        }
    }
}