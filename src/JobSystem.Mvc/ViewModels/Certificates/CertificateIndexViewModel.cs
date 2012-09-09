using System;
using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Certificates
{
	public class CertificateIndexViewModel
	{
		public Guid Id { get; set; }
		public string CertificateNo { get; set; }
		public string TypeName { get; set; }
		public string DateCreated { get; set; }
		public string CreatedBy { get; set; }
		public string JobItemNo { get; set; }
		public string JobNo { get; set; }
	}
}