using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
	public static class JobItemRepositoryTestHelper
	{
		public static IJobItemRepository GetJobItemRepository_StubsGetById_ReturnsNull(Guid jobItemId)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(jobItemId)).Return(null);
			return jobItemRepositoryStub;
		}

		public static IJobItemRepository GetJobItemRepository_StubsGetById_ReturnsJobItemOnPendingJob(Guid jobItemId)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(jobItemId)).Return(GetJobItem(jobItemId, true));
			return jobItemRepositoryStub;
		}

		public static IJobItemRepository GetJobItemRepository_StubsGetById_ReturnsJobItem(Guid jobItemId)
		{
			var jobItemRepositoryStub = MockRepository.GenerateStub<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(jobItemId)).Return(GetJobItem(jobItemId, false));
			return jobItemRepositoryStub;
		}

		private static JobItem GetJobItem(Guid jobItemId, bool isPending)
		{
			var createdBy = new UserAccount
			{
				Id = Guid.NewGuid(),
				EmailAddress = "chris.oneil@gmail.com",
				Name = "Chris O'Neil",
				JobTitle = "Development Engineer",
				Roles = UserRole.Manager | UserRole.Member,
				PasswordHash = "passwordhash",
				PasswordSalt = "passwordsalt"
			};
			return new JobItem
			{
				Id = jobItemId,
				Job = new Job
				{
					Id = Guid.NewGuid(),
					JobNo = "JR2000",
					CreatedBy = createdBy,
					OrderNo = "ORDER12345",
					DateCreated = DateTime.UtcNow,
					Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" },
					IsPending = isPending
				},
				ItemNo = 1,
				SerialNo = "12345",
				Instrument = new Instrument
				{
					Id = Guid.NewGuid(),
					Manufacturer = "Druck",
					ModelNo = "DPI601IS",
					Range = "None",
					Description = "Digital Pressure Indicator"
				},
				CalPeriod = 12,
				Created = DateTime.UtcNow,
				CreatedUser = createdBy,
			};
		}
	}
}