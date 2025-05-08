using System;
using System.Collections.Generic;

namespace RFIDDAL.Models
{
    public partial class CategoryType
    {
        public int CategoryTypeId { get; set; }
        public string CategoryTypeName { get; set; } = null!;
        public string? CategoryTypeCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
