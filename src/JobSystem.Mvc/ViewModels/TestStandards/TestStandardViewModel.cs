using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.TestStandards
{
	public class TestStandardViewModel
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		[Required]
		[Display(Name = "Serial Number")]
		public string SerialNo { get; set; }
		[Required]
		[Display(Name = "Certificate Number")]
		public string CertificateNo { get; set; }
	}
}