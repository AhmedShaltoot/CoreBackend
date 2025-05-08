using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using RFIDAPI.Authentication;
using RFIDBLL.DTOs;
using RFIDBLL.Services.Contracts;
using RFIDBLL.Services.Services;

namespace RFIDAPI.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CrosOriginPolicy")]
    //[Authorize(Roles = "SuperAdmin")]  // used role based authorization
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        IUserService _userService;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IUserService userService, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
            _passwordHasher = passwordHasher;

        }

        [HttpGet("get-all-users-pagerold")]
        public async Task<dynamic> GetAllUsersPagerOld(bool isActive, int pageSize, int currentPage, string? keyword)
        {
            var res = _userService.GetAllUsersPagerOld(isActive, pageSize, currentPage, keyword);
            return res;
        }
        
        [HttpGet("get-all-users-pager")]
        public async Task<dynamic> GetAllUsersPager(bool isActive, int pageSize, int currentPage, string? keyword)
        {
            var res = _userService.GetAllUsersPager(isActive, pageSize, currentPage, keyword);
            return res;
        }
        [HttpGet("get-user-by-id")]
        public object GetUserById(int userId)
        {
            return _userService.GetUserById(userId);
        }

        [HttpPost("add-user")]
        public bool AddUser(UserDTO user)
        {
            var passwordHashed = _passwordHasher.HashPassword(null, user.Password);
            var tt =  _userService.AddUser(user, passwordHashed);

            return tt;
        }
        [HttpPut("update-user")]
        public bool UpdateUser(UserDTO user)
        {
            var passwordHashed = (user.Password != null && user.Password != "") ? _passwordHasher.HashPassword(null, user.Password) : null;
            var res = _userService.UpdateUser(user, passwordHashed);
            return res;
        }
        //[HttpGet]
        //[Route("get-allusers")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _userManager.Users
        //        .Select(user => new UserDTO { Id = user.Id, UserName = user.UserName, Email = user.Email, Roles = _userManager.GetRolesAsync(user).Result })
        //        .ToListAsync();

        //    return Ok(users);
        //}
        [HttpDelete("delete-user")]
        public bool DeleteUser(int userId)
        {
            return _userService.DeleteUser(userId);
        }

        [HttpGet]
        [Route("get-alltechnicians-ddl")]
        public async Task<IActionResult> GetAllTechniciansDDL()
        {
            var users = await _userManager.Users.Where(c=> c.RoleName == "فنى")
                .Select(user => new { UserId = user.UserId, UserName = user.UserName })
                .ToListAsync();

            return Ok(users);
        }
        [HttpGet]
        [Route("get-allusers-ddl")]
        public async Task<IActionResult> GetAllUsersDDL()
        {
            var users = await _userManager.Users.Where(c => c.RoleName != "فنى")
                .Select(user => new { UserId = user.UserId, UserName = user.UserName })
                .ToListAsync();

            return Ok(users);
        }
        [HttpGet]
        [Route("get-user-roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();

            var viewUser = new UserRolesDTO
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new CheckBoxSelectedDTO
                {
                    DisplayValue = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return Ok(viewUser);
        }
        [HttpGet]
        [Route("get-userRoles-withPermissions")]
        public async Task<IActionResult> GetUserRolesWithPermissions(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var userroles = await _userManager.GetRolesAsync(user);
            if (userroles.Count == 0)
                return NotFound();
            List<RoleWithClaims> RolesWithClaims = new List<RoleWithClaims>();
            foreach (var role in userroles)
            {
                var roleObj = _roleManager.FindByNameAsync(role).Result;
                var roleClaims = _roleManager.GetClaimsAsync(roleObj).Result.Select(c => c.Value).ToList();
                RoleWithClaims RoleWithClaims = new RoleWithClaims
                {
                    RoleId = roleObj.Id,
                    RoleName = roleObj.Name,
                    RoleCalims = roleClaims
                };
                RolesWithClaims.Add(RoleWithClaims);
            }
            var allUserRolesWithClaims = new UserRolesPermissionsDTO
            {
                UserId = user.Id,
                UserName = user.UserName,
                RolesWithClaims = RolesWithClaims
            };

            return Ok(allUserRolesWithClaims);
        }

        //[HttpPost]
        //[Route("update-user-roles")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateUserRoles(UserRolesDTO user)
        //{
        //    var user = await _userManager.FindByIdAsync(user.UserId);

        //    if (user == null)
        //        return NotFound();

        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    await _userManager.RemoveFromRolesAsync(user, userRoles);
        //    await _userManager.AddToRolesAsync(user, user.Roles.Where(r => r.IsSelected).Select(r => r.DisplayValue));

        //    //foreach (var role in user.Roles)
        //    //{
        //    //    if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
        //    //        await _userManager.RemoveFromRoleAsync(user, role.RoleName);

        //    //    if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
        //    //        await _userManager.AddToRoleAsync(user, role.RoleName);
        //    //}

        //    return Ok(new Response { Status = "Success", Message = "Roles Updated successfully!" });
        //}
    }
}