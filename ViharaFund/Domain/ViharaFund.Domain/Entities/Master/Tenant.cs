﻿using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Master
{
    public class Tenant : BaseEntity
    {
        public string OrganizationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Package { get; set; } = string.Empty;
    }
}
