using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests.Helpers
{
	public class JobItemServiceFactory
	{
		public static JobItemService Create(
			Guid jobId, Guid instrumentId, Guid initialStatusId, Guid locationId, Guid fieldId, int jobItemCount)
		{
			return Create(
				MockRepository.GenerateMock<IJobItemRepository>(), jobId, instrumentId, initialStatusId, locationId, fieldId, jobItemCount,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobItemService Create(
			IJobItemRepository jobItemRepository, Guid jobId, Guid instrumentId, Guid initialStatusId, Guid locationId, Guid fieldId, int jobItemCount)
		{
			return Create(
				jobItemRepository, jobId, instrumentId, initialStatusId, locationId, fieldId, jobItemCount,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobItemService Create(
			IJobItemRepository jobItemRepository, Guid jobId, Guid instrumentId, Guid initialStatusId, Guid locationId, Guid fieldId, int jobItemCount, IUserContext userContext)
		{
			return new JobItemService(
				userContext,
				GetJobRepository(jobId, jobItemCount),
				jobItemRepository,
				GetInstrumentRepository(instrumentId),
				GetListItemRepository(initialStatusId, locationId, fieldId),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
		}

		private static IJobRepository GetJobRepository(Guid jobId, int jobItemCount)
		{
			var jobRepository = MockRepository.GenerateStub<IJobRepository>();
			if (jobId != Guid.Empty)
				jobRepository.Stub(x => x.GetById(jobId)).Return(GetJob(jobId));
			else
				jobRepository.Stub(x => x.GetById(jobId)).Return(null);
			jobRepository.Stub(x => x.GetJobItemCount(jobId)).Return(jobItemCount);
			return jobRepository;
		}

		private static IInstrumentRepository GetInstrumentRepository(Guid instrumentId)
		{
			var instrumentRepositoryStub = MockRepository.GenerateStub<IInstrumentRepository>();
			if (instrumentId != Guid.Empty)
				instrumentRepositoryStub.Stub(x => x.GetById(instrumentId)).Return(GetInstrument(instrumentId));
			else
				instrumentRepositoryStub.Stub(x => x.GetById(instrumentId)).Return(null);
			return instrumentRepositoryStub;
		}

		private static IListItemRepository GetListItemRepository(Guid initialStatusId, Guid locationId, Guid fieldId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			if (initialStatusId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(initialStatusId)).Return(GetInitialStatus(initialStatusId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(initialStatusId)).Return(null);
			if (locationId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(locationId)).Return(GetLocation(locationId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(locationId)).Return(null);
			if (fieldId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(fieldId)).Return(GetField(fieldId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(fieldId)).Return(null);
			listItemRepositoryStub.Stub(x => x.GetByName("Booked In")).Return(GetBookedInStatus());
			return listItemRepositoryStub;
		}

		private static Job GetJob(Guid jobId)
		{
			return new Job
			{
				Id = jobId,
				DateCreated = DateTime.Now,
				JobNo = "JR2000",
				CreatedBy = TestUserContext.CreateAdminUser(),
				OrderNo = "ORDERNO1000",
				AdviceNo = "ADVICENO1000",
				Customer = GetCustomer(),
				Type = GetJobType(),
				Contact = "Job Contact",
				Instructions = "some instructions",
				Notes = "job notes",
				IsPending = true
			};
		}

		private static ListItem GetJobType()
		{
			return new ListItem
			{
				Id = Guid.NewGuid(),
				Name = "Lab Services",
				Type = ListItemType.JobType
			};
		}

		private static ListItem GetBookedInStatus()
		{
			return new ListItem
			{
				Id = Guid.NewGuid(),
				Name = "Booked In",
				Type = ListItemType.JobItemStatus
			};
		}

		private static Customer GetCustomer()
		{
			return new Customer
			{
				Id = Guid.NewGuid(),
				Name = "EMIS (UK) Ltd"
			};
		}

		private static Instrument GetInstrument(Guid instrumentId)
		{
			return new Instrument
			{
				Id = instrumentId,
				Manufacturer = "Druck",
				ModelNo = "DPI601IS",
				Range = "None",
				Description = "Digital Pressure Indicator"
			};
		}

		private static ListItem GetInitialStatus(Guid initialStatusId)
		{
			return new ListItem
			{
				Id = initialStatusId,
				Name = "UKAS Calibration",
				Type = ListItemType.JobItemInitialStatus
			};
		}

		private static ListItem GetLocation(Guid locationId)
		{
			return new ListItem
			{
				Id = locationId,
				Name = "UKAS Calibration",
				Type = ListItemType.JobItemLocation
			};
		}

		private static ListItem GetField(Guid fieldId)
		{
			return new ListItem
			{
				Id = fieldId,
				Name = "E - Electrical",
				Type = ListItemType.JobItemField
			};
		}
	}
}