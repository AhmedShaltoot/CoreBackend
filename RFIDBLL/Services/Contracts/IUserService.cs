using RFIDBLL.DTOs;
using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Contracts
{
    public interface IUserService
    {
        object GetAllUsersPager(bool isActive, int pageSize, int currentPage, string keyword = "");
        object GetAllUsersPagerOld(bool isActive, int pageSize, int currentPage, string keyword = "");
        //object UsersDDL();
        UserDTO GetUserById(int UserId);
        bool AddUser(UserDTO User, string PasswordHashed);
        bool UpdateUser(UserDTO user, string? PasswordHashed);
        bool DeleteUser(int UserId);
    }
}
