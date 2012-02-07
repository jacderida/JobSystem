using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Consignments
{
	public class ConsignmentCreateViewModel
	{
		public Guid Id { get; set; }
		[Display(Name="Supplier")]
		public Guid SupplierId { get; set; }
		public string Instructions { get; set; }
	}
}