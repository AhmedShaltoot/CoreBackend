using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.DTOs
{
    public class PermissionGroupDto
    {
        public string PageName { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }
    }

}
