using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.DTOs
{
    public class RoleDTO
    {
        
        public string RoleId { get; set; }
        [Required, StringLength(256)]
        public string Name { get; set; }
    }
}
