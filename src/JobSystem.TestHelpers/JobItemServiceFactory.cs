	using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.TestHelpers.Context;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using Rhino.Mocks;

namespace JobSystem.TestHelpers
{
	public class JobItemServiceFactory
	{
		public static JobItemService Create(IUserContext userContext, IJobItemRepository jobItemRepository)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new JobItemService(
				userContext,
				MockRepository.GenerateStub<IJobRepository>(),
				jobItemRepository,
				new ListItemService(userContext, MockRepository.GenerateStub<IListItemRepository>(), dispatcher),
				new InstrumentService(userContext, MockRepository.GenerateStub<IInstrumentRepository>(), dispatcher),
				dispatcher);
		}

		public static JobItemService Create(Guid jobId, Guid instrumentId, Guid initialStatusId, Guid fieldId, int jobItemCount)
		{
			return Create(
				MockRepository.GenerateMock<IJobItemRepository>(), jobId, instrumentId, initialStatusId, fieldId, jobItemCount,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobItemService Create(
			IJobItemRepository jobItemRepository, Guid jobId, Guid instrumentId, Guid initialStatusId, Guid fieldId, int jobItemCount)
		{
			return Create(
				jobItemRepository, jobId, instrumentId, initialStatusId, fieldId, jobItemCount,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobItemService Create(
			IJobItemRepository jobItemRepository, Guid jobId, Guid instrumentId, Guid initialStatusId, Guid fieldId, int jobItemCount, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new JobItemService(
				userContext,
				GetJobRepository(jobId, jobItemCount),
				jobItemRepository,
				new ListItemService(userContext, GetListItemRepository(initialStatusId, fieldId), dispatcher),
				new InstrumentService(userContext, GetInstrumentRepository(instrumentId), dispatcher),
				dispatcher);
		}

		public static JobItemService CreateForAddWorkItem(IJobItemRepository jobItemRepository, Guid workStatusId, Guid workTypeId, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new JobItemService(
				userContext,
				MockRepository.GenerateStub<IJobRepository>(),
				jobItemRepository,
				new ListItemService(userContext, GetListItemRepositoryForAddWorkItem(workStatusId, workTypeId), dispatcher),
				new InstrumentService(userContext, MockRepository.GenerateStub<IInstrumentRepository>(), dispatcher),
				dispatcher);
		}

		public static JobItemService CreateForAddWorkItem(IJobRepository jobRepository, IJobItemRepository jobItemRepository, Guid workStatusId, Guid workTypeId, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new JobItemService(
				userContext,
				jobRepository,
				jobItemRepository,
				new ListItemService(userContext, GetListItemRepositoryForAddWorkItem(workStatusId, workTypeId), dispatcher),
				new InstrumentService(userContext, MockRepository.GenerateStub<IInstrumentRepository>(), dispatcher),
				dispatcher);
		}

		public static JobItemService CreateForAddWorkItem(IJobItemRepository jobItemRepository, IListItemRepository listItemRepository, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new JobItemService(
				userContext,
				MockRepository.GenerateStub<IJobRepository>(),
				jobItemRepository,
				new ListItemService(userContext, listItemRepository, dispatcher),
				new InstrumentService(userContext, MockRepository.GenerateStub<IInstrumentRepository>(), dispatcher),
				dispatcher);
		}

		public static JobItemService CreateToReturnJobItem(IJobItemRepository jobItemRepository, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new JobItemService(
				userContext,
				MockRepository.GenerateStub<IJobRepository>(),
				jobItemRepository,
				new ListItemService(userContext, MockRepository.GenerateStub<IListItemRepository>(), dispatcher),
				new InstrumentService(userContext, MockRepository.GenerateStub<IInstrumentRepository>(), dispatcher),
				dispatcher);
		}

		public static JobItemService CreateForUpdateStatus(IJobItemRepository jobItemRepository, Guid statusId)
		{
			return CreateForUpdateStatus(jobItemRepository,GetListItemRepositoryForUpdateStatus(statusId),
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobItemService CreateForUpdateStatus(IJobItemRepository jobItemRepository, Guid statusId, IUserContext userContext)
		{
			return CreateForUpdateStatus(jobItemRepository, GetListItemRepositoryForUpdateStatus(statusId), userContext);
		}

		public static JobItemService CreateForUpdateStatus(IJobItemRepository jobItemRepository, IListItemRepository listItemRepository)
		{
			return CreateForUpdateStatus(jobItemRepository, listItemRepository,
				TestUserContext.Create("test@usercontext.com", "Test User", "Operations Manager", UserRole.Member));
		}

		public static JobItemService CreateForUpdateStatus(
			IJobItemRepository jobItemRepository, IListItemRepository listItemRepository, IUserContext userContext)
		{
			var dispatcher = MockRepository.GenerateStub<IQueueDispatcher<IMessage>>();
			return new JobItemService(
				userContext,
				MockRepository.GenerateStub<IJobRepository>(),
				jobItemRepository,
				new ListItemService(userContext, listItemRepository, dispatcher),
				new InstrumentService(userContext, MockRepository.GenerateStub<IInstrumentRepository>(), dispatcher),
				dispatcher);
		}

		public static IListItemRepository GetListItemRepositoryForAddWorkItem(Guid workStatusId, Guid workTypeId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			if (workStatusId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(workStatusId)).Return(GetAddWorkItemWorkStatus(workStatusId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(workStatusId)).Return(null);
			if (workTypeId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(workTypeId)).Return(GetAddWorkItemWorkType(workTypeId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(workTypeId)).Return(null);
			return listItemRepositoryStub;
		}

		public static ListItem GetAddWorkItemWorkStatus(Guid statusId)
		{
			return new ListItem
			{
				Id = statusId,
				Name = "Calibrated",
				Type = ListItemType.WorkStatusCalibrated,
				Category = new ListItemCategory
				{
					Id = Guid.NewGuid(),
					Name = "Status",
					Type = ListItemCategoryType.JobItemWorkStatus
				}
			};
		}

		public static ListItem GetAddWorkItemWorkType(Guid workTypeId)
		{
			return new ListItem
			{
				Id = workTypeId,
				Name = "Calibration",
				Type = ListItemType.WorkTypeCalibration,
				Category = new ListItemCategory
				{
					Id = Guid.NewGuid(),
					Name = "Work Type",
					Type = ListItemCategoryType.JobItemWorkType
				}
			};
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

		private static IListItemRepository GetListItemRepository(Guid initialStatusId, Guid fieldId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			if (initialStatusId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(initialStatusId)).Return(GetInitialStatus(initialStatusId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(initialStatusId)).Return(null);
			if (fieldId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(fieldId)).Return(GetField(fieldId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(fieldId)).Return(null);
			listItemRepositoryStub.Stub(x => x.GetByName("Booked In")).Return(GetBookedInStatus());
			return listItemRepositoryStub;
		}

		private static IListItemRepository GetListItemRepositoryForUpdateStatus(Guid statusId)
		{
			var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
			if (statusId != Guid.Empty)
				listItemRepositoryStub.Stub(x => x.GetById(statusId)).Return(GetStatusForUpdate(statusId));
			else
				listItemRepositoryStub.Stub(x => x.GetById(statusId)).Return(null);
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
				Type = ListItemType.JobTypeService
			};
		}

		private static ListItem GetBookedInStatus()
		{
			return new ListItem
			{
				Id = Guid.NewGuid(),
				Name = "Booked In",
				Type = ListItemType.StatusBookedIn
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
				Type = ListItemType.InitialStatusUkasCalibration
			};
		}

		private static ListItem GetField(Guid fieldId)
		{
			return new ListItem
			{
				Id = fieldId,
				Name = "E - Electrical",
				Type = ListItemType.CategoryElectrical
			};
		}

		private static ListItem GetStatusForUpdate(Guid statusId)
		{
			return new ListItem
			{
				Id = statusId,
				Name = "Calibrated",
				Type = ListItemType.WorkStatusCalibrated
			};
		}
	}
}