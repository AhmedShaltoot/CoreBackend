using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Contracts
{
    public interface ICategoryTypeService
    {
        object GetAllCategoryTypePager(bool isActive, int pageSize, int currentPage, string keyword = "");
        object GetCategoryTypeDDL();
        object GetCategoryTypeById(int modelId);
        bool AddCategoryType(CategoryType model);
        bool UpdateCategoryType(CategoryType model);
        bool DeleteCategoryType(int modelId);
    }
}
