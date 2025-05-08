using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.DTOs
{
    public class LoginedUserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string? RoleName { get; set; }
        public List<string> Permissions { get; set; }
    }
}
