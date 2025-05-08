using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RFIDBLL.Services.Contracts;
using RFIDDAL.Models;

namespace RFIDAPI.Controllers
{
    [EnableCors("CrosOriginPolicy")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryTypeController : ControllerBase
    {
        ICategoryTypeService _categoryTypeService;
        public CategoryTypeController(ICategoryTypeService categoryTypeService) 
        {
            _categoryTypeService = categoryTypeService;
        }

        [HttpGet("get-all-categoryType-pager")]
        public object GetAllCategoryTypePager(bool isActive, int pageSize, int currentPage, string? keyword) 
        {
            return _categoryTypeService.GetAllCategoryTypePager(isActive, pageSize, currentPage, keyword);
        }

        [HttpGet("get-categoryType-ddl")]
        public object GetCategoryTypeDDL()
        {
           return _categoryTypeService.GetCategoryTypeDDL();
        }

        [HttpGet("get-categoryType-by-id")]
        public object GetCategoryTypeById(int categoryTypeId)
        {
            return _categoryTypeService.GetCategoryTypeById(categoryTypeId);
        }

        [HttpPost("add-categoryType")]
        public bool AddCategoryType(CategoryType categoryType)
        {
            return _categoryTypeService.AddCategoryType(categoryType);
        }

        [HttpPut("update-categoryType")]
        public bool UpdateCategoryType(CategoryType categoryType)
        {
            return _categoryTypeService.UpdateCategoryType(categoryType);
        }

        [HttpDelete("delete-categoryType")]
        public bool DeleteCategoryType(int categoryTypeId)
        {
            return _categoryTypeService.DeleteCategoryType(categoryTypeId);
        }
    }
}
