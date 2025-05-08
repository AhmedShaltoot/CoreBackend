using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.DTOs
{
    public class UserRolesPermissionsDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleWithClaims> RolesWithClaims { get; set; }
    }
}
