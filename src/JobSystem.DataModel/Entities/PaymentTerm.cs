using System;

namespace JobSystem.DataModel.Entities
{
	public class PaymentTerm
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
	}
}