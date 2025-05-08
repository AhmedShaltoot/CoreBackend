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
    public class StatusService : IStatusService
    {
        IRepositoryWrapper _repositoryWrapper;
        public StatusService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public object GetAllStatusesPager(bool isActive, int pageSize, int currentPage, string keyword = "")
        {
            return _repositoryWrapper.Status.FindByConditionPager(currentPage, pageSize, m => m.IsActive == isActive && (string.IsNullOrEmpty(keyword) || m.StatusNameAr.ToLower().Contains(keyword.ToLower())));
        }

        public object GetStatusesDDL()
        {
            return _repositoryWrapper.Status.FindAll().Select(m => new
            {
                m.StatusId,
                m.StatusNameAr,
                m.StatusNameEn
            }).ToList();
        }

        public object GetStatusById(int statusId) 
        {
            return _repositoryWrapper.Status.FindByCondition(m=>m.StatusId ==statusId).FirstOrDefault();
        }
        public bool AddStatus(Status status) 
        {
            try
            {
                status.StatusNameEn = "Test";
                _repositoryWrapper.Status.Create(status);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateStatus(Status status)
        {
            try
            {
                status.StatusNameEn = "Test";
                var statusDB = _repositoryWrapper.Status.FindById(status.StatusId);
                _repositoryWrapper.MapObjects<Status>(status,statusDB);
                _repositoryWrapper.Status.Update(statusDB);
                _repositoryWrapper.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteStatus(int statusId) 
        {
            try
            {
                var statusDB = _repositoryWrapper.Status.FindById(statusId);
                _repositoryWrapper.Status.Delete(statusDB);
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
