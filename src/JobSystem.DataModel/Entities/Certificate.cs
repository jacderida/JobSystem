using System;
using System.Collections.Generic;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public class Certificate
	{
		public virtual Guid Id { get; set; }
		public virtual string CertificateNumber { get; set; }
		public virtual UserAccount CreatedBy { get; set; }
		public virtual DateTime DateCreated { get; set; }
		public virtual ListItem Type { get; set; }
		public virtual JobItem JobItem { get; set; }
		public virtual string ProcedureList { get; set; }
		public virtual IList<TestStandard> TestStandards { get; set; }

		public Certificate()
		{
			TestStandards = new List<TestStandard>();
		}
	}
}