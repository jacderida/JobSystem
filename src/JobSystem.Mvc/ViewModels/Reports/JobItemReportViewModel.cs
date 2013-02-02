using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Reports
{
    public class JobItemReportViewModel
    {
        public Guid StatusId { get; set; }
        public IEnumerable<SelectListItem> Status { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<SelectListItem> Customer { get; set; }
    }
}