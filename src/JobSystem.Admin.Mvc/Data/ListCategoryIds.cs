using System;

namespace JobSystem.Admin.Mvc.Data
{
	public static class ListCategoryIds
	{
		public static Guid JobTypeId = Guid.NewGuid();
		public static Guid CategoryId = Guid.NewGuid();
		public static Guid InitialStatusId = Guid.NewGuid();
		public static Guid StatusId = Guid.NewGuid();
		public static Guid WorkTypeId = Guid.NewGuid();
		public static Guid WorkStatusId = Guid.NewGuid();
		public static Guid PaymentTermId = Guid.NewGuid();
		public static Guid CertificateId = Guid.NewGuid();
	}
}