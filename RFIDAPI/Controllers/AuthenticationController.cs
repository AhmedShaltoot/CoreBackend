using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using RFIDAPI.Authentication;
using System.Xml.Xsl;
using RFIDDAL.Models;
using RFIDBLL.Services.Contracts;
using RFIDDAL.Repositories.Contracts;
using Newtonsoft.Json;
using RFIDBLL.DTOs;
using DocumentFormat.OpenXml.Math;

namespace RFIDAPI.Controllers
{
    [EnableCors("CrosOriginPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        protected IRepositoryWrapper _repoWrapper;
        private readonly IRoleService _roleService;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IRepositoryWrapper repoWrapper, IRoleService roleService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _repoWrapper = repoWrapper;
            _roleService = roleService;
        }
        #region Password Verification
        private bool VerifyCheckPassword(string password, string hashedPassword)
        {
            if (hashedPassword.StartsWith("$2y$")) // Assuming PHP's Hash::make uses bcrypt algorithm
            {
                return VerifyPhpHash(password, hashedPassword);
            }
            else // Assuming ASP.NET Core's PasswordHasher format
            {
                return VerifyAspNetCoreHash(password, hashedPassword);
            }
        }

        private bool VerifyPhpHash(string password, string hashedPassword)
        {
            var res = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return res;
        }

        private bool VerifyAspNetCoreHash(string password, string hashedPassword)
        {
            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            return passwordHasher.VerifyHashedPassword(null, hashedPassword, password) == PasswordVerificationResult.Success;
        }
        #endregion
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = _repoWrapper.User.FindByCondition(a => a.UserName == model.Username).FirstOrDefault();

                if (user == null) { return Unauthorized(); }
                var result = VerifyCheckPassword(model.Password, user.PasswordHash);
                if (!result)
                {
                    return Unauthorized();
                }
                else
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleName),
                };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(20),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    var roleId = _repoWrapper.UserRole.FindByCondition(c => c.UserId == user.Id).FirstOrDefault().RoleId;
                    //var roleId = roleManager.FindByNameAsync(user.RoleName).Result.Id;
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        user = EncodeToBase64(new LoginedUserDTO
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            RoleName = user.RoleName,
                            RoleId = roleId,
                            Permissions = _roleService.GetUserPermissions(roleId).Result
                        })
                    });
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        //[HttpPost]
        //[Route("loginOld")]
        //public async Task<IActionResult> LoginOld([FromBody] LoginModel model)
        //{
        //    var user = await userManager.FindByNameAsync(model.Username);
        //    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var userRoles = await userManager.GetRolesAsync(user);

        //        var authClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        //            new Claim(ClaimTypes.Role, user.RoleName),
        //        };

        //        //foreach (var userRole in userRoles)
        //        //{
        //        //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        //}

        //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        //        //var secret = Environment.GetEnvironmentVariable("JWT:MyProject_JWT_Secret");

        //        var token = new JwtSecurityToken(
        //            issuer: _configuration["JWT:ValidIssuer"],
        //            audience: _configuration["JWT:ValidAudience"],
        //            expires: DateTime.Now.AddHours(20),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //            );
        //        var roleId = _repoWrapper.UserRole.FindByCondition(c => c.UserId == user.Id).FirstOrDefault().RoleId;

        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo,
        //            user = EncodeToBase64(new LoginedUserDTO
        //            {
        //                UserId = user.UserId,
        //                UserName = user.UserName,
        //                RoleName = user.RoleName,
        //                RoleId = roleId,
        //                Permissions = _roleService.GetUserPermissions(roleId).Result
        //            })
        //        });
        //    }
        //    return Unauthorized();
        //}
        
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            // Validate the refresh token
            var principal = ValidateRefreshToken(refreshTokenRequest.RefreshToken);

            if (principal == null)
            {
                return Unauthorized();
            }

            var newAccessToken = GenerateAccessToken(principal.Claims);

            return Ok(new
            {
                token = newAccessToken,
                expiration = DateTime.Now.AddHours(3)
            });
        }

        private ClaimsPrincipal ValidateRefreshToken(string refreshToken)
        {
            // Validate the refresh token against your storage
            // If valid, return a ClaimsPrincipal representing the user
            // Otherwise, return null
            return null;
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            //var hashPass = userManager.PasswordHasher.HashPassword(user, model.Password);
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                //await userManager.AddToRoleAsync(user, roleManager.FindByIdAsync(model.RoleId).Result.Name);
                //await userManager.AddToRoleAsync(user, model.RoleId);
                AspNetUserRole aspNetUserRole = new AspNetUserRole()
                {
                    UserId = user.Id,
                    RoleId = model.RoleId
                };
                RFIDdbContext _TestContext = new RFIDdbContext();
                _TestContext.AspNetUserRoles.Add(aspNetUserRole);
                try
                {
                    _TestContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        [Authorize]
        [Route("is-admin")]
        [HttpGet]
        public async Task<IActionResult> IsAdmin()
        {
            var y = MyEnum.add | MyEnum.edit | MyEnum.delete;
            var z = y.HasFlag(MyEnum.add);
            var a = y.HasFlag(MyEnum.read);
            var b = (int)y;

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var x = userManager.GetLoginsAsync(user).Result;
            var role = userManager.GetRolesAsync(user).Result;

            if (role.Count > 0 && role[0] == "Admin")
            {
                return Ok(true);
            }
            else
                return Ok(false);
        }
        [Flags]
        enum MyEnum
        {
            read,
            add,
            edit,
            delete
        }

        public static string EncodeToBase64(LoginedUserDTO user)
        {
            string json = JsonConvert.SerializeObject(user);

            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            return Convert.ToBase64String(jsonBytes);
        }
    }
}
