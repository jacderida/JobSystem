using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Currencies;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class CurrencyServiceTests
	{
		private Currency _savedCurrency;
		private CurrencyService _currencyService;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;
		private Guid _currencyForEditId;
		private Currency _currencyForEdit;

		[SetUp]
		public void Setup()
		{
			_savedCurrency = null;
			_domainValidationException = null;
			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
			_currencyForEditId = Guid.NewGuid();
			_currencyForEdit = new Currency
			{
				Id = _currencyForEditId,
				Name = "CAD",
				DisplayMessage = "All prices in Canadian Dollars"
			};
		}

		#region Create

		[Test]
		public void Create_ValidCurrencyDetails_CurrencyCreated()
		{
			var id = Guid.NewGuid();
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryMock = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			CreateCurrency(id, name, displayMessage);
			currencyRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(id, _savedCurrency.Id);
			Assert.AreEqual(name, _savedCurrency.Name);
			Assert.AreEqual(displayMessage, _savedCurrency.DisplayMessage);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_NoIdSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			CreateCurrency(id, name, displayMessage);
		}

		[Test]
		public void Create_NameNotSupplied_DomainValidationExceptionThrow()
		{
			var id = Guid.NewGuid();
			var name = String.Empty;
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			CreateCurrency(id, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.NameNotSupplied));
		}

		[Test]
		public void Create_NameLargerThan50Characters_DomainValidationExceptionThrow()
		{
			var id = Guid.NewGuid();
			var name = new string('a', 51);
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			CreateCurrency(id, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.NameTooLarge));
		}

		[Test]
		public void Create_DisplayMessageNotSupplied_DomainValidationExceptionThrow()
		{
			var id = Guid.NewGuid();
			var name = "CAD";
			var displayMessage = String.Empty;

			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			CreateCurrency(id, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DisplayMessageNotSupplied));
		}

		[Test]
		public void Create_DisplayMessageTooLarge_DomainValidationExceptionThrow()
		{
			var id = Guid.NewGuid();
			var name = "CAD";
			var displayMessage = new string('a', 51);

			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			CreateCurrency(id, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DisplayMessageTooLarge));
		}

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrow()
		{
			var id = Guid.NewGuid();
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member);
			var currencyRepositoryStub = MockRepository.GenerateStub<ICurrencyRepository>();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			CreateCurrency(id, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		public void CreateCurrency(Guid id, string name, string displayMessage)
		{
			try
			{
				_savedCurrency = _currencyService.Create(id, name, displayMessage);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region Edit

		[Test]
		public void Edit_ValidCurrencyDetails_CurrencyEdit()
		{
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryMock = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryMock.Stub(x => x.GetById(_currencyForEditId)).Return(_currencyForEdit);
			currencyRepositoryMock.Expect(x => x.Update(null)).IgnoreArguments();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			currencyRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(name, _currencyForEdit.Name);
			Assert.AreEqual(displayMessage, _currencyForEdit.DisplayMessage);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Edit_IdNotSupplied_CurrencyEdit()
		{
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(_currencyForEditId)).Return(_currencyForEdit);
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(Guid.NewGuid(), name, displayMessage);
		}

		[Test]
		public void Edit_NameNotSupplied_DomainValidationExceptionThrow()
		{
			var name = String.Empty;
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(_currencyForEditId)).Return(_currencyForEdit);
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.NameNotSupplied));
		}

		[Test]
		public void Edit_NameLargerThan50Characters_DomainValidationExceptionThrow()
		{
			var name = new string('a', 51);
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(_currencyForEditId)).Return(_currencyForEdit);
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.NameTooLarge));
		}

		[Test]
		public void Edit_DisplayMessageNotSupplied_DomainValidationExceptionThrow()
		{
			var name = "CAD";
			var displayMessage = String.Empty;

			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(_currencyForEditId)).Return(_currencyForEdit);
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DisplayMessageNotSupplied));
		}

		[Test]
		public void Edit_DisplayMessageThan50Characters_DomainValidationExceptionThrow()
		{
			var name = "CAD";
			var displayMessage = new string('a', 51);

			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(_currencyForEditId)).Return(_currencyForEdit);
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.DisplayMessageTooLarge));
		}

		[Test]
		public void Edit_EditSystemGBPCurrency_DomainValidationExceptionThrow()
		{
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(_currencyForEditId)).Return(
				new Currency { Id = _currencyForEditId, Name = "GBP", DisplayMessage = "All prices in GBP" });
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.EditProhibited));
		}

		[Test]
		public void Edit_EditSystemEURCurrency_DomainValidationExceptionThrow()
		{
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(_currencyForEditId)).Return(
				new Currency { Id = _currencyForEditId, Name = "EUR", DisplayMessage = "All prices in Euros" });
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.EditProhibited));
		}

		[Test]
		public void Edit_EditSystemUSDCurrency_DomainValidationExceptionThrow()
		{
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			currencyRepositoryStub.Stub(x => x.GetById(_currencyForEditId)).Return(
				new Currency { Id = _currencyForEditId, Name = "USD", DisplayMessage = "All prices in US Dollars" });
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.EditProhibited));
		}

		[Test]
		public void Edit_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrow()
		{
			var name = "CAD";
			var displayMessage = "All prices in Canadian Dollars";

			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member);
			var currencyRepositoryStub = MockRepository.GenerateMock<ICurrencyRepository>();
			_currencyService = new CurrencyService(_userContext, currencyRepositoryStub, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			EditCurrency(_currencyForEditId, name, displayMessage);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		public void EditCurrency(Guid id, string name, string displayName)
		{
			try
			{
				_currencyForEdit = _currencyService.Edit(id, name, displayName);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetById

		[Test]
		public void GetById_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public);
			_currencyService = new CurrencyService(
				_userContext,
				MockRepository.GenerateStub<ICurrencyRepository>(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			GetById(Guid.NewGuid());
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		public void GetById(Guid id)
		{
			try
			{
				_currencyService.GetById(id);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetCurrencies

		[Test]
		public void GetCurrencies_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public);
			_currencyService = new CurrencyService(
				_userContext,
				MockRepository.GenerateStub<ICurrencyRepository>(),
				MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			GetCurrencies();
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(Messages.InsufficientSecurityClearance));
		}

		public void GetCurrencies()
		{
			try
			{
				_currencyService.GetCurrencies();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}