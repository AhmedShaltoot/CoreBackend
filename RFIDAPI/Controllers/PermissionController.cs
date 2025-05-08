
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
    public class PermissionController : ControllerBase
    {
        IPermissionService _cityHistoryService;

        public PermissionController(IPermissionService cityHistoryService)
        {
            _cityHistoryService = cityHistoryService;
        }

        [HttpGet("get-all-permission-pager")]
        public object GetAllPermissionPager(bool isActive = true, int pageSize = 25, int currentPage = 1, string? keyword = "")
        {
            return _cityHistoryService.GetAllPermissionPager(isActive, pageSize, currentPage, keyword);
        }

        [HttpGet("get-permission-ddl")]
        public object GetPermissionDDL() 
        {
            return _cityHistoryService.GetPermissionDDL();
        }

        [HttpGet("get-permission-by-id")]
        public object GetPermissionById(int permissionId)
        {
            return _cityHistoryService.GetPermissionById(permissionId);
        }

        [HttpPost("add-permission")]
        public bool AddPermission(Permission permission)
        {
            return _cityHistoryService.AddPermission(permission);
        }

        [HttpPut("update-permission")]
        public bool UpdatePermission(Permission permission)
        {
            return _cityHistoryService.UpdatePermission(permission);
        }

        [HttpDelete("delete-permission")]
        public bool DeletePermission(int permissionId)
        {
            return _cityHistoryService.DeletePermission(permissionId);
        }
    }
}