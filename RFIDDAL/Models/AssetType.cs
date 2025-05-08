using System;
using System.Collections.Generic;

namespace RFIDDAL.Models
{
    public partial class AssetType
    {
        public int AssetTypeId { get; set; }
        public int? ParentAssetTypeId { get; set; }
        public bool? IsActive { get; set; }
        public string AssetTypeName { get; set; } = null!;
        public string? AssetTypeCode { get; set; }
        public string? ParentAssetTypeName { get; set; }
        public string? ParentAssetTypeCode { get; set; }
        public string UniversityName { get; set; } = null!;
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? OdooId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
