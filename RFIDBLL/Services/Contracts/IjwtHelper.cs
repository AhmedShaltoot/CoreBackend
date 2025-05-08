using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.Services.Contracts
{
    public interface IjwtHelper
    {
        string GetUserIdFromToken(HttpContext httpContext);
        string GetRoleFromToken(HttpContext httpContext);
        bool CheckUserNoticePeriodExpirationFromToken(HttpContext httpContext, int noticePeriod);
    }
}
