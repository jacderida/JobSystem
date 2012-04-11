using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.DataModel.Entities;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Certificates
{
	public class CertificateViewModel
	{
		public Guid Id { get; set; }
		public Guid JobItemId { get; set; }

		[Display(Name = "Test Standard")]
		[Required]
		public Guid[] SelectedTestStandardIds { get; set; }
		[Display(Name = "Test Standard")]
		public IEnumerable<SelectListItem> TestStandards { get; set; }

		public Guid CertificateTypeId { get; set; }
		[Display(Name="Type")]
		public IEnumerable<SelectListItem> CertificateTypes { get; set; }
	}
}