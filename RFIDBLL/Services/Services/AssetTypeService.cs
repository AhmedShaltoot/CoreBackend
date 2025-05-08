using RFIDBLL.Services.Contracts;
using RFIDDAL.Models;
using RFIDDAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Services
{
    public class AssetTypeService : IAssetTypeService
    {
        IRepositoryWrapper _repositoryWrapper;
        public AssetTypeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public object GetAllAssetTypePager(bool isActive, int pageSize, int currentPage, int? categoryType, string keyword = "")
        {
            return _repositoryWrapper.AssetType.FindByConditionPager(currentPage, pageSize, m => m.IsDeleted != true && (m.ParentAssetTypeId == categoryType || categoryType == null || categoryType == 0) &&
            (string.IsNullOrEmpty(keyword) ||
                 m.AssetTypeName.ToLower().Contains(keyword.ToLower()) ||
                 m.AssetTypeCode.ToLower().Contains(keyword.ToLower()))
            );
        }

        public object GetAssetTypeDDL()
        {
            return _repositoryWrapper.AssetType.FindByCondition(m=> m.IsDeleted != true).Select(m => new
            {
                m.AssetTypeId,
                m.AssetTypeName
            }).ToList();
        }

        public object GetAssetTypeById(int categoryId)
        {
            return _repositoryWrapper.AssetType.FindByCondition(m => m.AssetTypeId == categoryId).FirstOrDefault();
        }
        public Tuple<bool, int> AddAssetType(AssetType category)
        {
            try
            {
                category.CreationDate = DateTime.Now;
                var catType = _repositoryWrapper.CategoryType.FindById((int)category.ParentAssetTypeId);
                category.ParentAssetTypeName = catType.CategoryTypeName;
                category.ParentAssetTypeCode = catType.CategoryTypeCode;
                _repositoryWrapper.AssetType.Create(category);
                _repositoryWrapper.Save();
                return new Tuple<bool, int>(true, category.AssetTypeId);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, int>(false, 0);
            }
        }
        public Tuple<bool, int> UpdateAssetType(AssetType category)
        {
            try
            {
                var categoryDB = _repositoryWrapper.AssetType.FindById(category.AssetTypeId);
                category.UniversityName = categoryDB.UniversityName;
                category.IsActive = categoryDB.IsActive;
                category.CreatedBy = categoryDB.CreatedBy;
                category.CreationDate = categoryDB.CreationDate;
                category.OdooId = categoryDB.OdooId;
                category.LastUpdatedDate = DateTime.Now;
                var catType = _repositoryWrapper.CategoryType.FindById((int)category.ParentAssetTypeId);
                category.ParentAssetTypeName = catType.CategoryTypeName;
                category.ParentAssetTypeCode = catType.CategoryTypeCode;
                _repositoryWrapper.MapObjects<AssetType>(category, categoryDB);
                _repositoryWrapper.AssetType.Update(categoryDB);
                _repositoryWrapper.Save();
                return new Tuple<bool, int>(true, (int)category.OdooId);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, int>(false, 0);
            }
        }
        public bool AddBulkAssetTypeFistTime(List<AssetType> assetTypes)
        {
            List<CategoryType> categoryTypes = new List<CategoryType>();
            foreach (var assetType in assetTypes)
            {
                categoryTypes.Add(new CategoryType() {CategoryTypeName = assetType.ParentAssetTypeName, CategoryTypeCode = assetType.ParentAssetTypeCode, IsActive = true });
            }
            _repositoryWrapper.CategoryType.CreateBulk(categoryTypes);
            _repositoryWrapper.Save();
            try
            {
                var categoryTypesDB = _repositoryWrapper.CategoryType.FindAll();
                foreach(var assetType in assetTypes)
                {
                    assetType.UniversityName = "جامعة نايف العربية للعلوم الأمنية";
                    assetType.IsActive = true;
                    assetType.CreatedBy = 1;
                    assetType.CreationDate = DateTime.Now;
                    assetType.ParentAssetTypeId = categoryTypesDB.Where(c=> c.CategoryTypeCode == assetType.ParentAssetTypeCode && c.CategoryTypeName == assetType.ParentAssetTypeName).FirstOrDefault().CategoryTypeId;
                }
                _repositoryWrapper.AssetType.CreateBulk(assetTypes);
                _repositoryWrapper.Save();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateAssetTypeOdooId(AssetType assetType)
        {
            try
            {
                var assetTypeDB = _repositoryWrapper.AssetType.FindById(assetType.AssetTypeId);
                assetTypeDB.OdooId = assetType.OdooId;
                _repositoryWrapper.AssetType.Update(assetTypeDB);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                MethodsMonitor methodsMonitor = new MethodsMonitor
                {
                    MethodId = 102,
                    RunningDate = DateTime.Now,
                    ExcutionTime = "5",
                    Status = false,
                    ErrorMessage = ex.Message,
                    ChartValue = 5,
                    CreatedBy = 1
                };
                AddMonitor(methodsMonitor);
                return false;
            }
        }

        public bool DeleteAssetType(int categoryId, int userId)
        {
            try
            {
                var categoryDB = _repositoryWrapper.AssetType.FindById(categoryId);
                categoryDB.DeletedBy = userId;
                categoryDB.DeletionDate = DateTime.Now;
                categoryDB.IsDeleted = true;
                _repositoryWrapper.AssetType.Update(categoryDB);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddMonitor(MethodsMonitor monitor)
        {

            _repositoryWrapper.Monitor.Create(monitor);
            _repositoryWrapper.Save();
            return true;

        }
    }
}
