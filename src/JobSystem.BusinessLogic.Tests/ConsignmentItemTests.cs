﻿using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Resources.Consignments;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class ConsignmentItemTests
	{
		private ConsignmentItemService _consignmentItemService;
		private JobItemService _jobItemService;
		private DomainValidationException _domainValidationException;
		private ConsignmentItem _savedConsigmentItem;
		private PendingConsignmentItem _savedPendingItem;
		private JobItem _jobItemToUpdate;
		private IUserContext _userContext;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member);
		}

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			_jobItemToUpdate = new JobItem
			{
				Id = Guid.NewGuid(),
				Job = new Job
				{
					Id = Guid.NewGuid(),
					JobNo = "JR2000",
					CreatedBy = _userContext.GetCurrentUser(),
					OrderNo = "ORDER12345",
					DateCreated = DateTime.UtcNow,
					Customer = new Customer { Id = Guid.NewGuid(), Name = "Gael Ltd" }
				},
				ItemNo = 1,
				SerialNo = "12345",
				Instrument = new Instrument
				{
					Id = Guid.NewGuid(), Manufacturer = "Druck", ModelNo = "DPI601IS", Range = "None", Description = "Digital Pressure Indicator"
				},
				CalPeriod = 12,
				Created = DateTime.UtcNow,
				CreatedUser = _userContext.GetCurrentUser(),
			};
		}

		[Test]
		public void Create_ValidDetailsSuppliedConsignmentHas0Items_SuccessfullyCreated()
		{
			var id = Guid.NewGuid();
			var consignmentId = Guid.NewGuid();
			var instructions = "Consignment instructions";

			var consignmentItemRepositoryMock = MockRepository.GenerateMock<IConsignmentItemRepository>();
			consignmentItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdate.Id)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdate.Id, 0, 0, String.Empty, ListItemType.StatusConsigned, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationSubContract));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_consignmentItemService = ConsignmentItemServiceFactory.Create(consignmentItemRepositoryMock, jobItemRepositoryMock, consignmentId, _userContext);
			_jobItemService = _jobItemService = JobItemServiceFactory.CreateToReturnJobItem(jobItemRepositoryMock, _userContext);
			CreateConsignmentItem(id, _jobItemToUpdate.Id, consignmentId, instructions);
			consignmentItemRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedConsigmentItem.Id);
			Assert.AreEqual(1, _savedConsigmentItem.ItemNo);
			Assert.AreEqual(ListItemType.StatusConsigned, _jobItemToUpdate.Status.Type);
			Assert.AreEqual(ListItemType.WorkLocationSubContract, _jobItemToUpdate.Location.Type);
		}

		[Test]
		public void Create_ValidDetailsSuppliedConsignmentHas1Items_SuccessfullyCreated()
		{
			var id = Guid.NewGuid();
			var consignmentId = Guid.NewGuid();
			var instructions = "Consignment instructions";

			var consignmentItemRepositoryMock = MockRepository.GenerateMock<IConsignmentItemRepository>();
			consignmentItemRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdate.Id)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.EmitItemHistory(
				_userContext.GetCurrentUser(), _jobItemToUpdate.Id, 0, 0, String.Empty, ListItemType.StatusConsigned, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationSubContract));
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate)).IgnoreArguments();

			_consignmentItemService = ConsignmentItemServiceFactory.Create(consignmentItemRepositoryMock, jobItemRepositoryMock, consignmentId, _userContext, 1);
			_jobItemService = _jobItemService = JobItemServiceFactory.CreateToReturnJobItem(jobItemRepositoryMock, _userContext);
			CreateConsignmentItem(id, _jobItemToUpdate.Id, consignmentId, instructions);
			consignmentItemRepositoryMock.VerifyAllExpectations();
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedConsigmentItem.Id);
			Assert.AreEqual(2, _savedConsigmentItem.ItemNo);
			Assert.AreEqual(ListItemType.StatusConsigned, _jobItemToUpdate.Status.Type);
			Assert.AreEqual(ListItemType.WorkLocationSubContract, _jobItemToUpdate.Location.Type);
		}

		[Test]
		public void Create_JobIsPending_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var consignmentId = Guid.NewGuid();
			var instructions = "Consignment instructions";

			_jobItemToUpdate.Job.IsPending = true;
			var jobItemRepositoryStub = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdate.Id)).Return(_jobItemToUpdate);

			_consignmentItemService = ConsignmentItemServiceFactory.Create(MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemRepositoryStub, consignmentId, _userContext);
			_jobItemService = _jobItemService = JobItemServiceFactory.CreateToReturnJobItem(jobItemRepositoryStub, _userContext);
			CreateConsignmentItem(id, _jobItemToUpdate.Id, consignmentId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.PendingJob));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var consignmentId = Guid.NewGuid();
			var instructions = "Consignment instructions";

			_jobItemToUpdate.Job.IsPending = true;
			var jobItemRepositoryStub = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryStub.Stub(x => x.GetById(_jobItemToUpdate.Id)).Return(_jobItemToUpdate);

			_consignmentItemService = ConsignmentItemServiceFactory.Create(MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemRepositoryStub, consignmentId, _userContext);
			_jobItemService = _jobItemService = JobItemServiceFactory.CreateToReturnJobItem(jobItemRepositoryStub, _userContext);
			CreateConsignmentItem(id, _jobItemToUpdate.Id, consignmentId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.PendingJob));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidConsignmentIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var consignmentId = Guid.Empty;
			var instructions = "Consignment instructions";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdate.Id)).Return(_jobItemToUpdate);

			_consignmentItemService = ConsignmentItemServiceFactory.Create(MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemRepositoryMock, consignmentId, _userContext);
			_jobItemService = _jobItemService = JobItemServiceFactory.CreateToReturnJobItem(jobItemRepositoryMock, _userContext);
			CreateConsignmentItem(id, _jobItemToUpdate.Id, consignmentId, instructions);
		}

		[Test]
		public void Create_InstructionsGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var consignmentId = Guid.NewGuid();
			var instructions = new string('a', 256);

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemToUpdate.Id)).Return(_jobItemToUpdate);

			_consignmentItemService = ConsignmentItemServiceFactory.Create(MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemRepositoryMock, consignmentId, _userContext);
			_jobItemService = _jobItemService = JobItemServiceFactory.CreateToReturnJobItem(jobItemRepositoryMock, _userContext);
			CreateConsignmentItem(id, _jobItemToUpdate.Id, consignmentId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InstructionsTooLarge));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidJobItemIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var consignmentId = Guid.NewGuid();
			var instructions = "Consignment instructions";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(Guid.NewGuid())).Return(null);

			_consignmentItemService = ConsignmentItemServiceFactory.Create(MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemRepositoryMock, consignmentId, _userContext);
			_jobItemService = _jobItemService = JobItemServiceFactory.CreateToReturnJobItem(jobItemRepositoryMock, _userContext);
			CreateConsignmentItem(id, _jobItemToUpdate.Id, consignmentId, instructions);
		}

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var consignmentId = Guid.NewGuid();
			var instructions = "Consignment instructions";

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Stub(x => x.GetById(Guid.NewGuid())).Return(null);

			_consignmentItemService = ConsignmentItemServiceFactory.Create(
				MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemRepositoryMock, consignmentId,
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			_jobItemService = _jobItemService = JobItemServiceFactory.CreateToReturnJobItem(jobItemRepositoryMock,
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			CreateConsignmentItem(id, _jobItemToUpdate.Id, consignmentId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void CreateConsignmentItem(Guid id, Guid jobItemId, Guid consignmentId, string instructions)
		{
			try
			{
				_savedConsigmentItem = _consignmentItemService.Create(id, jobItemId, consignmentId, instructions);
				_jobItemToUpdate = _jobItemService.GetById(_jobItemToUpdate.Id);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		[Test]
		public void CreatePending_ValidPendingItemDetails_PendingItemCreated()
		{
			var id = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = "Please consign this item";

			var consignmentItemRepositoryMock = MockRepository.GenerateMock<IConsignmentItemRepository>();
			consignmentItemRepositoryMock.Expect(x => x.CreatePendingItem(null)).IgnoreArguments();
			_consignmentItemService = ConsignmentItemServiceFactory.CreateForPendingItems(consignmentItemRepositoryMock, jobItemId, supplierId, _userContext);
			CreatePendingConsignmentItem(id, jobItemId, supplierId, instructions);
			consignmentItemRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedPendingItem.Id);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_InvalidJobItemId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = Guid.Empty;
			var supplierId = Guid.NewGuid();
			var instructions = "Please consign this item";

			_consignmentItemService = ConsignmentItemServiceFactory.CreateForPendingItems(
				MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemId, supplierId, _userContext);
			CreatePendingConsignmentItem(id, jobItemId, supplierId, instructions);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatePending_InvalidSupplierId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var supplierId = Guid.Empty;
			var instructions = "Please consign this item";

			_consignmentItemService = ConsignmentItemServiceFactory.CreateForPendingItems(
				MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemId, supplierId, _userContext);
			CreatePendingConsignmentItem(id, jobItemId, supplierId, instructions);
		}

		[Test]
		public void CreatePending_JobIsInPendingState_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = "Please consign this item";

			_consignmentItemService = ConsignmentItemServiceFactory.CreateForPendingItems(
				MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemId, supplierId, _userContext, true);
			CreatePendingConsignmentItem(id, jobItemId, supplierId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.PendingJob));
		}

		[Test]
		public void CreatePending_InstructionsGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = new string('a', 256);

			_consignmentItemService = ConsignmentItemServiceFactory.CreateForPendingItems(
				MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemId, supplierId, _userContext);
			CreatePendingConsignmentItem(id, jobItemId, supplierId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InstructionsTooLarge));
		}

		[Test]
		public void CreatePending_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var jobItemId = Guid.NewGuid();
			var supplierId = Guid.NewGuid();
			var instructions = "Please consign this item";

			_consignmentItemService = ConsignmentItemServiceFactory.CreateForPendingItems(
				MockRepository.GenerateStub<IConsignmentItemRepository>(), jobItemId, supplierId,
				 TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			CreatePendingConsignmentItem(id, jobItemId, supplierId, instructions);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		private void CreatePendingConsignmentItem(Guid id, Guid jobItemId, Guid supplierId, string instructions)
		{
			try
			{
				_savedPendingItem = _consignmentItemService.CreatePending(id, jobItemId, supplierId, instructions);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}