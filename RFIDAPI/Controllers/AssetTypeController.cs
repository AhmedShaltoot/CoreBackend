using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RFIDBLL.AutoMapperConfig;
using RFIDBLL.DTOs;
using RFIDBLL.Services.Contracts;
using RFIDBLL.Services.Services;
using RFIDDAL.Models;
using System.Net.Http;

namespace RFIDAPI.Controllers
{
    [EnableCors("CrosOriginPolicy")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypeController : ControllerBase
    {
        IAssetTypeService _assetTypeService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        protected IjwtHelper _jwtHelper;
        public AssetTypeController(IAssetTypeService assetTypeService, IHttpClientFactory httpClientFactory, IConfiguration configuration, IjwtHelper jwtHelper)
        {
            _assetTypeService = assetTypeService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
        }

        [HttpGet("get-all-assetType-pager")]
        public object GetAllAssetTypePager(bool isActive, int pageSize, int currentPage, int? categoryType, string? keyword)
        {
            return _assetTypeService.GetAllAssetTypePager(isActive, pageSize, currentPage, categoryType, keyword);
        }

        [HttpGet("get-assetType-ddl")]
        public object GetAssetTypeDDL()
        {
            return _assetTypeService.GetAssetTypeDDL();
        }

        [HttpGet("get-assetType-by-id")]
        public object GetAssetTypeById(int categoryId)
        {
            return _assetTypeService.GetAssetTypeById(categoryId);
        }

        [HttpPost("add-assetType")]
        public async Task<bool> AddAssetType(AssetType assetType)
        {
            var userId = _jwtHelper.GetUserIdFromToken(this.HttpContext);
            userId = userId == null ? "10" : userId;
            assetType.CreatedBy = int.Parse(userId);
            var res = _assetTypeService.AddAssetType(assetType);
            var callOdooApis = bool.Parse(_configuration["callOdooApis"].ToString());
            return true;
            //if (res.Item1 == true && callOdooApis == true)
            //{
            //    var odooResponse = await AddAssetTypeToOdoo(assetType);
            //    if (odooResponse != null && odooResponse.Status == "success")
            //    {
            //        assetType.OdooId = odooResponse.OdooId;
            //        assetType.AssetTypeId = res.Item2;
            //        _assetTypeService.UpdateAssetTypeOdooId(assetType);

            //        return true;
            //    }
            //    else
            //        return true;
            //}
            //else if (res.Item1 == true && callOdooApis == false)
            //    return true;
            //else return false;
        }

        [HttpPut("update-assetType")]
        public async Task<bool> UpdateAssetType(AssetType assetType)
        {
            var userId = _jwtHelper.GetUserIdFromToken(this.HttpContext);
            userId = userId == null ? "10" : userId;
            assetType.LastUpdatedBy = int.Parse(userId);
            var res = _assetTypeService.UpdateAssetType(assetType);
            var callOdooApis = bool.Parse(_configuration["callOdooApis"].ToString());
            return true;
            //if (res.Item1 == true && callOdooApis == true)
            //{
            //    assetType.OdooId = res.Item2;
            //    var odooResponse = await UpdateAssetTypeInOdoo(assetType);
            //    if (odooResponse != null && odooResponse.Result.Status == "success")
            //    {
            //        return true;
            //    }
            //    else return false;
            //}
            //else if (res.Item1 == true && callOdooApis == false)
            //    return true;
            //else
            //    return false;
        }

        [HttpDelete("delete-assetType")]
        public bool DeleteAssetType(int categoryId)
        {
            var userId = _jwtHelper.GetUserIdFromToken(this.HttpContext);
            userId = userId == null ? "10" : userId;
            var res = _assetTypeService.DeleteAssetType(categoryId, int.Parse(userId));
            return res;
        }
    }
}
