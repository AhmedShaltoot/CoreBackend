using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.EntityFrameworkCore;
using RFIDBLL.AutoMapperConfig;
using RFIDBLL.DTOs;
using RFIDBLL.Services.Contracts;
using RFIDDAL;
using RFIDDAL.Models;
using RFIDDAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Services
{
    public class PermissionService : IPermissionService
    {
        IRepositoryWrapper _repositoryWrapper;
        public PermissionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public object GetAllPermissionPager(bool isActive, int pageSize, int currentPage, string keyword = "")
        {
            var res = _repositoryWrapper.Permission.FindByConditionPager(currentPage, pageSize, c => c.IsActive == isActive &&
            (string.IsNullOrEmpty(keyword) || c.PermissionName.ToLower().Contains(keyword.ToLower()) || c.PageName.ToLower().Contains(keyword.ToLower())));
            return res;
        }
        public object GetPermissionDDL()
        {
            return _repositoryWrapper.Permission.FindAll().Select(c => new
            {
                c.PermissionId,
                c.PermissionName
            }).ToList();
        }
        public Permission GetPermissionById(int id)
        {
            var res = _repositoryWrapper.Permission.FindByCondition(c => c.PermissionId == id).FirstOrDefault();
            return res;
        }
        public bool AddPermission(Permission entity)
        {
            try
            {
                // Check if both PermissionName and PermissionNameAr are null
                if ((entity.PermissionName == null || entity.PermissionName == "") && (entity.PermissionNameAr == null || entity.PermissionNameAr == ""))
                {
                    // Create array of permission names in English and Arabic
                    string[] permissionNamesEn = new string[] { "Add", "View", "Edit", "Delete" };
                    string[] permissionNamesAr = new string[] { "اضافة", "عرض", "تعديل", "حذف" };

                    // Create and add 4 permission objects with the same page name
                    for (int i = 0; i < permissionNamesEn.Length; i++)
                    {
                        Permission newPermission = new Permission
                        {
                            PermissionName = permissionNamesEn[i],
                            PermissionNameAr = permissionNamesAr[i],
                            PageName = entity.PageName,
                            PageNameAr = entity.PageNameAr,
                            IsActive = entity.IsActive,
                            RolePermissions = entity.RolePermissions
                        };

                        _repositoryWrapper.Permission.Create(newPermission);
                        _repositoryWrapper.Save();
                    }
                }
                else
                {
                    _repositoryWrapper.Permission.Create(entity);
                    _repositoryWrapper.Save();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdatePermission(Permission entity)
        {
            try
            {
                entity.IsActive = true;
                var PermissionDB = GetPermissionById(entity.PermissionId);
                if (entity.PageNameAr != PermissionDB.PageNameAr || entity.PermissionName != PermissionDB.PermissionName || entity.PermissionNameAr != PermissionDB.PermissionNameAr)
                {
                    var allWithSamePage = _repositoryWrapper.Permission.FindByCondition(c => c.PermissionId != entity.PermissionId && c.PageName == PermissionDB.PageName).ToList();
                    foreach (var item in allWithSamePage)
                    {
                        item.PageNameAr = entity.PageNameAr;
                        item.PageName = entity.PageName;
                        item.PermissionName = item.PermissionName.Replace(item.PageName, "");
                        item.PermissionNameAr = item.PermissionName == "Add" ? "اضافة" : item.PermissionName == "Edit" ? "تعديل" : item.PermissionName == "View" ? "عرض" : item.PermissionName == "Delete" ? "حذف" : "";
                    }
                    _repositoryWrapper.Permission.UpdateBulk(allWithSamePage);
                    _repositoryWrapper.Save();
                }
                _repositoryWrapper.MapObjects<Permission>(entity, PermissionDB);


                _repositoryWrapper.Permission.Update(PermissionDB);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeletePermission(int id)
        {
            try
            {
                var PermissionDB = GetPermissionById(id);
                _repositoryWrapper.Permission.Delete(PermissionDB);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
