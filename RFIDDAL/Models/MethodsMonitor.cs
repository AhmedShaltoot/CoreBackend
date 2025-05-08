using System;
using System.Collections.Generic;

namespace RFIDDAL.Models
{
    public partial class MethodsMonitor
    {
        public int MonitorId { get; set; }
        public int MethodId { get; set; }
        public DateTime RunningDate { get; set; }
        public string? ExcutionTime { get; set; }
        public bool Status { get; set; }
        public string? ErrorMessage { get; set; }
        public int ChartValue { get; set; }
        public int? CreatedBy { get; set; }
    }
}
