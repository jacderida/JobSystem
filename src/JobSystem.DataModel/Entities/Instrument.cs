using System;
namespace JobSystem.DataModel.Entities
{
	public class Instrument
	{
		public virtual Guid Id { get; set; }
		public virtual string Manufacturer { get; set; }
		public virtual string ModelNo { get; set; }
		public virtual string Range { get; set; }
		public virtual string Description { get; set; }
	}
}