using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using NUnit.Framework;
using JobSystem.BusinessLogic.Tests.Context;
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
		public void CreateConsignmentItem_ValidDetailsSupplied_SuccessfullyCreated()
		{
		}
	}
}