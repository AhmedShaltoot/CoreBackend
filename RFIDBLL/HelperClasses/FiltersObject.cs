using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.HelperClasses
{
    public class FiltersObject
    {
        public long? ProjectId { get; set; }
        public long? DepartmentId { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreationDateFrom { get; set; }
        public DateTime? CreationDateTo { get; set; }
    }
}
