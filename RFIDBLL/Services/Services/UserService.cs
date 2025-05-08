using DocumentFormat.OpenXml.Spreadsheet;
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
    public class UserService : IUserService
    {
        protected IRepositoryWrapper _repoWrapper;
        public UserService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public object GetAllUsersPager(bool isActive, int pageSize, int currentPage, string keyword = "")
        {
            var res = _repoWrapper.User.FindUsersWithRolesByConditionPager(currentPage, pageSize, m => (m.IsActive == isActive && m.IsDeleted != true) && (string.IsNullOrEmpty(keyword) || m.UserName.ToLower().Contains(keyword.ToLower())));
            return res;
        }
        public object GetAllUsersPagerOld(bool isActive, int pageSize, int currentPage, string keyword = "")
        {
            var res = _repoWrapper.User.FindByConditionPager(currentPage, pageSize, m => (m.IsActive == isActive) && (string.IsNullOrEmpty(keyword) || m.UserName.ToLower().Contains(keyword.ToLower())));
            return res;
        }
        public object GetUsersDDL()
        {
            return _repoWrapper.User.FindAll().Select(m => new
            {
                m.UserId,
                m.UserName,
                m.AspNetUserRoles.FirstOrDefault().Role.Name
            }).ToList();
        }

        public UserDTO GetUserById(int userId)
        {
            var res= _repoWrapper.User.FindByCondition(m => m.UserId == userId).FirstOrDefault();
            return OMapper.Mapper.Map<UserDTO>(res);
        }
        public bool UpdateUser(UserDTO user, string? PasswordHashed)
        {
            try
            {
                var userDB = _repoWrapper.User.FindByCondition(u=> u.UserId == user.UserId).FirstOrDefault();
                userDB.UserName = user.UserName;
                userDB.Email = user.Email;
                userDB.PhoneNumber = user.PhoneNumber;
                userDB.RoleName = (user.RoleName != null && user.RoleName != "") ? user.RoleName : user.RoleId == "b66de06e-619a-4f16-be9e-9f44fe9c8053" ? "فنى" : "";
                if(PasswordHashed != null && PasswordHashed != "")
                    userDB.PasswordHash = PasswordHashed;
                
                _repoWrapper.User.Update(userDB);

                var userRoles = _repoWrapper.UserRole.FindByCondition(c => c.UserId == userDB.Id).FirstOrDefault();
                if(userRoles.RoleId != user.RoleId)
                {
                    userRoles.RoleId = user.RoleId;
                    _repoWrapper.UserRole.Update(userRoles);
                }
                _repoWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddUser(UserDTO user, string PasswordHashed)
        {
            try
            {
                //var usermodel = OMapper.Mapper.Map<AspNetUser>(user);
                var newuser = new AspNetUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = user.Email,
                    UserName = user.UserName,
                    RoleName =(user.RoleName != null && user.RoleName != "") ? user.RoleName : user.RoleId == "b66de06e-619a-4f16-be9e-9f44fe9c8053" ? "فنى" : "",
                    PasswordHash = PasswordHashed,
                    SecurityStamp = GenerateSecurityStamp(),
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,  
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    IsActive = true,
                    NormalizedUserName = user.UserName.ToUpper(), // Set NormalizedUserName
                    NormalizedEmail = user.Email.ToUpper(), // Set NormalizedEmail
                    ConcurrencyStamp = Guid.NewGuid().ToString(), // Set ConcurrencyStamp
                    //CreatedAt = DateTime.Now
                };
                _repoWrapper.User.Create(newuser);
                _repoWrapper.Save();

                AspNetUserRole aspNetUserRole = new AspNetUserRole
                {
                    RoleId = user.RoleId,
                    UserId = newuser.Id
                };
                _repoWrapper.UserRole.Create(aspNetUserRole);
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
        public bool DeleteUser(int userId)
        {
            try
            {
                var userDB = _repoWrapper.User.FindByCondition(u=> u.UserId == userId).FirstOrDefault();
                userDB.IsDeleted = true;
                userDB.DeletedDate = DateTime.Now;
                userDB.DeletedBy = 25;
                _repoWrapper.User.Update(userDB);
                _repoWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
