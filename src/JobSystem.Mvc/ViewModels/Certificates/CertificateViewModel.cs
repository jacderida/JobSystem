using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Certificates
{
	public class CertificateViewModel
	{
		public Guid Id { get; set; }
		public Guid JobItemId { get; set; }
		public Guid[] SelectedTestStandardIds { get; set; }
		public MultiSelectList TestStandards { get; set; }
		public Guid CertificateTypeId { get; set; }
		public IEnumerable<SelectListItem> CertificateTypes { get; set; }
	}
}