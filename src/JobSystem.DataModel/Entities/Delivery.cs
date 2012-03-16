using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class Delivery
	{
		public virtual Guid Id { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual UserAccount CreatedBy { get; set; }
		public virtual string Fao { get; set; }
	}
}