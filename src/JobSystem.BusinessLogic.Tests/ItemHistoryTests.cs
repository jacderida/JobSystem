using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Tests.Context;
using JobSystem.BusinessLogic.Tests.Helpers;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.DataModel;

namespace JobSystem.BusinessLogic.Tests
{
	[TestFixture]
	public class ItemHistoryTests
	{
		private JobItemService _jobItemService;
		private DomainValidationException _domainValidationException;
		private ItemHistory _savedItemHistory;
		private Guid _jobItemId = Guid.NewGuid();
		private JobItem _jobItemToUpdate;
		private IUserContext _userContext;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member);
		}

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_jobItemToUpdate = new JobItem
			{
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
				Instrument = new Instrument { Id = Guid.NewGuid(), Manufacturer = "Druck", ModelNo = "DPI601IS", Range = "None", Description = "Digital Pressure Indicator" },
				CalPeriod = 12,
				Created = DateTime.UtcNow,
				CreatedUser = _userContext.GetCurrentUser(),
			};
		}

		[Test]
		public void CreateItemHistory_ValidItemHistoryDetails_ItemHistorySuccessfullyCreated()
		{
			var workStatusId = Guid.NewGuid();
			var workTypeId = Guid.NewGuid();
			var workLocationId = Guid.NewGuid();

			var jobItemRepositoryMock = MockRepository.GenerateMock<IJobItemRepository>();
			jobItemRepositoryMock.Expect(x => x.CreateItemHistory(null)).IgnoreArguments();
			jobItemRepositoryMock.Stub(x => x.GetById(_jobItemId)).Return(_jobItemToUpdate);
			jobItemRepositoryMock.Expect(x => x.Update(_jobItemToUpdate));
			_jobItemService = JobItemServiceFactory.CreateForItemHistory(jobItemRepositoryMock, workStatusId, workLocationId, workTypeId, _userContext);
			CreateItemHistory(Guid.NewGuid(), _jobItemId, 10, 10, "Instrument calibrated", workStatusId, workTypeId, workLocationId);
			jobItemRepositoryMock.VerifyAllExpectations();
			Assert.That(_savedItemHistory.Id != Guid.Empty);
			Assert.AreEqual(_savedItemHistory.DateCreated, _dateCreated);
			Assert.AreEqual("graham.robertson@intertek.com", _savedItemHistory.User.EmailAddress);
			Assert.AreEqual(workStatusId, _jobItemToUpdate.Status.Id);
			Assert.AreEqual(workLocationId, _jobItemToUpdate.Location.Id);
		}

		private void CreateItemHistory(Guid id, Guid jobItemId, int workTime, int overTime, string report, Guid workStatusId, Guid workTypeId, Guid workLocationId)
		{
			try
			{
				_savedItemHistory = _jobItemService.CreateItemHistory(id, jobItemId, workTime, overTime, report, workStatusId, workTypeId, workLocationId);
				_jobItemToUpdate = _jobItemService.GetById(_jobItemId);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}
	}
}