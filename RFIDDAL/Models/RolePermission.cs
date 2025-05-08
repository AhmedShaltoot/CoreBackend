using System;
using System.Collections.Generic;

namespace RFIDDAL.Models
{
    public partial class RolePermission
    {
        public int PermissionId { get; set; }
        public string RoleId { get; set; } = null!;

        public virtual Permission Permission { get; set; } = null!;
    }
}
