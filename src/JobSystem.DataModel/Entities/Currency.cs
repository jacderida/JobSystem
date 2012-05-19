using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.DataModel.Entities
{
	public class Currency
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string DisplayMessage { get; set; }
	}
}