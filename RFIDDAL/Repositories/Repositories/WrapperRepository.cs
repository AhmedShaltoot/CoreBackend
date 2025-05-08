using RFIDDAL.Models;
using RFIDDAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace RFIDDAL.Repositories.Repositories
{
    public class WrapperRepository : IRepositoryWrapper
    {
        private readonly RFIDdbContext _repoContext;

        private IUserRepository _user;
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        private IAssetTypeRepository _assetType;
        public IAssetTypeRepository AssetType
        {
            get
            {
                if (_assetType == null)
                {
                    _assetType = new AssetTypeRepository(_repoContext);
                }
                return _assetType;
            }
        }

        private ICategoryTypeRepository _categoryType;
        public ICategoryTypeRepository CategoryType
        {
            get
            {
                if (_categoryType == null)
                {
                    _categoryType = new CategoryTypeRepository(_repoContext);
                }
                return _categoryType;
            }
        }

        private IMonitorRepository _monitor;
        public IMonitorRepository Monitor
        {
            get
            {
                if (_monitor == null)
                {
                    _monitor = new MonitorRepository(_repoContext);
                }
                return _monitor;
            }
        }

        private IUserRoleRepository _userRole;
        public IUserRoleRepository UserRole
        {
            get
            {
                if (_userRole == null)
                {
                    _userRole = new UserRoleRepository(_repoContext);
                }
                return _userRole;
            }
        }
        private IRoleRepository _role;
        public IRoleRepository Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_repoContext);
                }
                return _role;
            }
        }
        private IPermissionRepository _permission;
        public IPermissionRepository Permission
        {
            get
            {
                if (_permission == null)
                {
                    _permission = new PermissionRepository(_repoContext);
                }
                return _permission;
            }
        }
        private IStatusRepository _status;
        public IStatusRepository Status
        {
            get
            {
                if (_status == null)
                {
                    _status = new StatusRepository(_repoContext);
                }
                return _status;
            }
        }
        private IRolePermissionRepository _rolePermission;
        public IRolePermissionRepository RolePermission
        {
            get
            {
                if (_rolePermission == null)
                {
                    _rolePermission = new RolePermissionRepository(_repoContext);
                }
                return _rolePermission;
            }
        }
        public WrapperRepository(RFIDdbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
        public T MapObjects<T>(T source, T target) where T : class, new()
        {
            var properties = typeof(T).GetProperties();
            foreach (PropertyInfo sourceProp in properties)
            {
                PropertyInfo targetProp = properties.Where(p => p.Name == sourceProp.Name).FirstOrDefault();
                if (targetProp != null && targetProp.GetType().Name == sourceProp.GetType().Name)
                {
                    targetProp.SetValue(target, sourceProp.GetValue(source));
                }
            }
            return target;
        }
        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
