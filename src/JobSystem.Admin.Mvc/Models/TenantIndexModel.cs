using System;

namespace JobSystem.Admin.Mvc.Models
{
    public class TenantIndexModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
    }
}