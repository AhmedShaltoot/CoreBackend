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
    public class StatusController : ControllerBase
    {
        IStatusService _statusService;
        public StatusController(IStatusService statusService) 
        {
            _statusService = statusService;
        }

        [HttpGet("get-all-statuses-pager")]
        public object GetAllStatusesPager(bool isActive, int pageSize, int currentPage, string? keyword) 
        {
            return _statusService.GetAllStatusesPager(isActive, pageSize, currentPage, keyword);
        }

        [HttpGet("get-statuses-ddl")]
        public object GetStatusesDDL()
        {
           return _statusService.GetStatusesDDL();
        }

        [HttpGet("get-status-by-id")]
        public object GetStatusById(int statusId)
        {
            return _statusService.GetStatusById(statusId);
        }

        [HttpPost("add-status")]
        public bool AddStatus(Status status)
        {
            return _statusService.AddStatus(status);
        }

        [HttpPut("update-status")]
        public bool UpdateStatus(Status status)
        {
            return _statusService.UpdateStatus(status);
        }

        [HttpDelete("delete-status")]
        public bool DeleteStatus(int statusId)
        {
            return _statusService.DeleteStatus(statusId);
        }
    }
}
