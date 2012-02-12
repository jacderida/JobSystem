using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Consignments;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class PendingConsignmentItem
	{
		public virtual Guid Id { get; set; }
		public virtual Supplier Supplier { get; set; }
		public virtual JobItem JobItem { get; set; }
		[StringLength(255, ErrorMessageResourceName = "InstructionsTooLarge", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Instructions { get; set; }
	}
}