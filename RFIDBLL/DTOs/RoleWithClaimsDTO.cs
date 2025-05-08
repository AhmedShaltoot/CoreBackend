using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.DTOs
{
    public class RoleWithClaimsDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<CheckBoxSelectedDTO> RoleCalims { get; set; }
    }
}
