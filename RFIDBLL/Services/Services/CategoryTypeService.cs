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
    public class CategoryTypeService : ICategoryTypeService
    {
        IRepositoryWrapper _repositoryWrapper;
        public CategoryTypeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public object GetAllCategoryTypePager(bool isActive, int pageSize, int currentPage, string keyword = "")
        {
            return _repositoryWrapper.CategoryType.FindByConditionPager(currentPage, pageSize, m => (string.IsNullOrEmpty(keyword) || m.CategoryTypeName.ToLower().Contains(keyword.ToLower())));
        }

        public object GetCategoryTypeDDL()
        {
            return _repositoryWrapper.CategoryType.FindAll().Select(m => new
            {
                m.CategoryTypeId,
                m.CategoryTypeName
            }).ToList();
        }

        public object GetCategoryTypeById(int categoryTypeId) 
        {
            return _repositoryWrapper.CategoryType.FindByCondition(m=>m.CategoryTypeId ==categoryTypeId).FirstOrDefault();
        }
        public bool AddCategoryType(CategoryType categoryType) 
        {
            try
            {
                _repositoryWrapper.CategoryType.Create(categoryType);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateCategoryType(CategoryType categoryType)
        {
            try
            {
                var categoryTypeDB = _repositoryWrapper.CategoryType.FindById(categoryType.CategoryTypeId);
                _repositoryWrapper.MapObjects<CategoryType>(categoryType,categoryTypeDB);
                _repositoryWrapper.CategoryType.Update(categoryTypeDB);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteCategoryType(int categoryTypeId) 
        {
            try
            {
                var categoryTypeDB = _repositoryWrapper.CategoryType.FindById(categoryTypeId);
                _repositoryWrapper.CategoryType.Delete(categoryTypeDB);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
