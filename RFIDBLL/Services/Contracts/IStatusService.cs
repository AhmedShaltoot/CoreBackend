using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Contracts
{
    public interface IStatusService
    {
        object GetAllStatusesPager(bool isActive, int pageSize, int currentPage, string keyword = "");
        object GetStatusesDDL();
        object GetStatusById(int modelId);
        bool AddStatus(Status model);
        bool UpdateStatus(Status model);
        bool DeleteStatus(int modelId);
    }
}
