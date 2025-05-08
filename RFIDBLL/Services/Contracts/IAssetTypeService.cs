using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Contracts
{
    public interface IAssetTypeService
    {
        object GetAllAssetTypePager(bool isActive, int pageSize, int currentPage, int? CategoryType, string keyword = "");
        object GetAssetTypeDDL();
        object GetAssetTypeById(int modelId);
        Tuple<bool, int> AddAssetType(AssetType model);
        Tuple<bool, int> UpdateAssetType(AssetType model);
        bool AddBulkAssetTypeFistTime(List<AssetType> assetTypes);
        bool DeleteAssetType(int modelId, int userId);
        bool UpdateAssetTypeOdooId(AssetType assetType);
    }
}
