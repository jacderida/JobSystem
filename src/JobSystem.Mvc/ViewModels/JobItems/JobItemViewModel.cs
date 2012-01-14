using System;

namespace JobSystem.Mvc.ViewModels.JobItems
{
	public class JobItemViewModel
	{
		public Guid Id { get; set; }
		public Guid JobId { get; set; }
		public Guid InstrumentId { get; set; }
		public string SerialNo { get; set; }
		public string AssetNo { get; set; }
		public Guid InitialStatusId { get; set; }
		public Guid LocationId { get; set; }
		public Guid FieldId { get; set; }
		public int CalPeriod { get; set; }
		public string Instructions { get; set; }
		public string Accessories { get; set; }
		public bool IsReturned { get; set; }
		public string ReturnReason { get; set; }
		public string Comments { get; set; }
	}
}
