using System;

namespace JobSystem.DataModel.Entities
{
	[Flags]
	public enum UserRole
	{
		None = 0,
		Member = 1,
		JobApprover = 2,
		OrderApprover = 4,
		Admin = 8,
		Manager = 16,
		Public = 32
	}
}