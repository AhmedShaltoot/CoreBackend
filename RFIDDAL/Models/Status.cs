using System;
using System.Collections.Generic;

namespace RFIDDAL.Models
{
    public partial class Status
    {
        public int StatusId { get; set; }
        public string StatusNameAr { get; set; } = null!;
        public string StatusNameEn { get; set; } = null!;
        public bool IsBabStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
