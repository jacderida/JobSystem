using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.Delivery;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class Delivery
	{
		public virtual Guid Id { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual UserAccount CreatedBy { get; set; }
		[StringLength(255, ErrorMessageResourceName = "InvalidFao", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Fao { get; set; }
	}
}