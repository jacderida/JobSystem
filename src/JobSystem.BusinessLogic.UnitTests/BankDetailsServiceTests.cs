using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;
using JobSystem.Resources.CompanyDetails;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class BankDetailsServiceTests
	{
		private BankDetails _savedBankDetails;
		private BankDetailsService _bankDetailsService;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;

		[SetUp]
		public void Setup()
		{
			_domainValidationException = null;
			_savedBankDetails = null;
			_userContext = TestUserContext.Create(
				"graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
		}

		#region Create

		[Test]
		public void Create_ValidBankDetails_BankDetailsCreated()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			bankDetailsRepositoryMock.VerifyAllExpectations();
			Assert.AreEqual(id, _savedBankDetails.Id);
			Assert.AreEqual(name, _savedBankDetails.Name);
			Assert.AreEqual(shortName, _savedBankDetails.ShortName);
			Assert.AreEqual(accountNo, _savedBankDetails.AccountNo);
			Assert.AreEqual(sortCode, _savedBankDetails.SortCode);
			Assert.AreEqual(address1, _savedBankDetails.Address1);
			Assert.AreEqual(address2, _savedBankDetails.Address2);
			Assert.AreEqual(address3, _savedBankDetails.Address3);
			Assert.AreEqual(address4, _savedBankDetails.Address4);
			Assert.AreEqual(address5, _savedBankDetails.Address5);
			Assert.AreEqual(iban, _savedBankDetails.Iban);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_IdNotSupplied_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
		}

		[Test]
		public void Create_NameNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = String.Empty;
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.NameRequired));
		}

		[Test]
		public void Create_NameGreaterThan255Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = new string('a', 256);
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.NameTooLarge));
		}

		[Test]
		public void Create_ShortNameNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = String.Empty;
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.ShortNameRequired));
		}

		[Test]
		public void Create_ShortNameGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = new string('a', 51);
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.ShortNameTooLarge));
		}

		[Test]
		public void Create_AccountNoNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = String.Empty;
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.AccountNoRequired));
		}

		[Test]
		public void Create_AccountNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = new string('a', 51);
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.AccountNoTooLarge));
		}

		[Test]
		public void Create_SortCodeNotSupplied_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = String.Empty;
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.SortCodeRequired));
		}

		[Test]
		public void Create_SortCodeGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = new string('a', 51);
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.SortCodeTooLarge));
		}

		[Test]
		public void Create_Address1GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = new string('a', 51);
			var address2 = "Johnstone";
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.AddressTooLarge));
		}

		[Test]
		public void Create_Address2GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = new string('a', 51);
			var address3 = "Renfewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.AddressTooLarge));
		}

		[Test]
		public void Create_Address3GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = new string('a', 51);
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.AddressTooLarge));
		}

		[Test]
		public void Create_Address4GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfrewshire";
			var address4 = new string('a', 51);
			var address5 = "Address5";
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.AddressTooLarge));
		}

		[Test]
		public void Create_Address5GreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfrewshire";
			var address4 = "Address4";
			var address5 = new string('a', 51);
			var iban = "IBAN number";

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.AddressTooLarge));
		}

		[Test]
		public void Create_IbanGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var name = "Royal Bank of Scotland";
			var shortName = "RBS";
			var accountNo = "00131184";
			var sortCode = "801652";
			var address1 = "High Street";
			var address2 = "Johnstone";
			var address3 = "Renfrewshire";
			var address4 = "Address4";
			var address5 = "Address5";
			var iban = new string('a', 51);

			var bankDetailsRepositoryMock = MockRepository.GenerateMock<IBankDetailsRepository>();
			bankDetailsRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_bankDetailsService = new BankDetailsService(
				_userContext, bankDetailsRepositoryMock, MockRepository.GenerateStub<IQueueDispatcher<IMessage>>());
			Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(BankDetailsMessages.IbanTooLarge));
		}

		private void Create(
			Guid id, string name, string shortName, string accountNo, string sortCode, string address1, string address2, string address3, string address4, string address5, string iban)
		{
			try
			{
				_savedBankDetails = _bankDetailsService.Create(id, name, shortName, accountNo, sortCode, address1, address2, address3, address4, address5, iban);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion

	}
}