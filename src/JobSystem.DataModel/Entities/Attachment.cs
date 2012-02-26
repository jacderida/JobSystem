using System;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class Attachment
	{
		public virtual Guid Id { get; set; }
		public virtual string Filename { get; set; }
	}
}