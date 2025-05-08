using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.DTOs
{
    public class SetRolePermissionDTO
    {
        public string roleId { get; set; }
        public List<int> permissionIds { get; set; }
    }
}
