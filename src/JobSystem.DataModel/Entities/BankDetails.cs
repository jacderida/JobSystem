using System;

namespace JobSystem.DataModel.Entities
{
	public class BankDetails
	{
		public virtual Guid Id { get; set; }
		public virtual string ShortName { get; set; }
		public virtual string AccountNo { get; set; }
		public virtual string SortCode { get; set; }
		public virtual string Name { get; set; }
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string Address3 { get; set; }
		public virtual string Address4 { get; set; }
		public virtual string Address5 { get; set; }
		public virtual string Iban { get; set; }
	}
}