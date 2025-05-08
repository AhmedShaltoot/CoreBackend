using System;
using System.Collections.Generic;

namespace RFIDDAL.Models
{
    public partial class Permission
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        public int PermissionId { get; set; }
        public string? PermissionName { get; set; } = null!;
        public string? PermissionNameAr { get; set; }
        public string? PageName { get; set; } = null!;
        public string? PageNameAr { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
