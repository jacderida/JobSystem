using System;
using System.ComponentModel;

namespace JobSystem.DataModel.Entities
{
	[Serializable]
	public enum ListItemCategoryType
	{
		[Description("Job Type")]
		JobType = 1,
		[Description("Category")]
		JobItemCategory = 2,
		[Description("Initial Status")]
		JobItemInitialStatus = 3,
		[Description("Status")]
		JobItemStatus = 4,
		[Description("Work Type")]
		JobItemWorkType = 6,
		[Description("Status")]
		JobItemWorkStatus = 7,
		[Description("Currency")]
		Currency = 8,
		[Description("Payment Term")]
		PaymentTerm = 9,
		[Description("Certificate")]
		Certificate = 11
	}
}