using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.DTOs
{
    public class RoleWithClaims
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> RoleCalims { get; set; }
    }
}
