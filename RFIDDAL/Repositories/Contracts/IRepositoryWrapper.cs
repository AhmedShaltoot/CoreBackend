using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using RFIDDAL.Repositories.Repositories;

namespace RFIDDAL.Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserRoleRepository UserRole { get; }
        IRoleRepository Role { get; }
        IPermissionRepository Permission { get; }
        IRolePermissionRepository RolePermission { get; }
        IStatusRepository Status { get; }
        IAssetTypeRepository AssetType { get; }
        ICategoryTypeRepository CategoryType { get; }
        IMonitorRepository Monitor { get; }
        T MapObjects<T>(T source, T target) where T : class, new();
        void Save();
    }
}
