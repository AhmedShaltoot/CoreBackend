//using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.HelperClasses
{
    public class HelperStruct
    {
        public class SearchKeyStruct
        {
            public const string Project = "Project";
            public const string Department = "Department";
            public const string ReqNo = "ReqNo";
            public const string ReqType = "ReqType";
            public const string Status = "Status";
        }

        public enum ClientPaidTypeEnum  // mapped to Table >> ClientPaidTypes
        {
            Paymob = 1,
            Manually = 2,
            FOC = 3
        }
        public enum ClientStatusEnum  // mapped to Table >> ClientStatus
        {
            Pending = 2,
            InPayment = 3,
            Confirmed = 4,
            Rejected = 5,
            Cancelled = 6
        }
    }
}
