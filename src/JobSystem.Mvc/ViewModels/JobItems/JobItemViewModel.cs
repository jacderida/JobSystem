using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.JobItems
{
	public class JobItemViewModel
	{
		public Guid Id { get; set; }
		public Guid JobId { get; set; }
		[Display(Name = "Serial Number")]
		public string SerialNo { get; set; }
		[Display(Name = "Asset Number")]
		public string AssetNo { get; set; }
		[Display(Name = "Calibration Period")]
		public int CalPeriod { get; set; }
		public string Instructions { get; set; }
		public string Accessories { get; set; }
		public bool IsReturned { get; set; }
		public string ReturnReason { get; set; }
		public string Comments { get; set; }
		[Display(Name = "Instrument")]
		public IEnumerable<SelectListItem> Instruments { get; set; }
		[Display(Name = "Status")]
		public IEnumerable<SelectListItem> Status { get; set; }
		[Display(Name = "Location")]
		public IEnumerable<SelectListItem> Locations { get; set; }
		[Display(Name = "Field")]
		public IEnumerable<SelectListItem> Fields { get; set; }
		public Guid InstrumentId { get; set; }
		public Guid InitialStatusId { get; set; }
		public Guid LocationId { get; set; }
		public Guid FieldId { get; set; }
	}
}
