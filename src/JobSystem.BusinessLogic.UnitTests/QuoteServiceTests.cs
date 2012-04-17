using System;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using JobSystem.TestHelpers.RepositoryHelpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
	[TestFixture]
	public class QuoteServiceTests
	{
		private Quote _savedQuote;
		private QuoteService _quoteService;
		private DomainValidationException _domainValidationException;
		private IUserContext _userContext;
		private DateTime _dateCreated = new DateTime(2011, 12, 29);

		[SetUp]
		public void Setup()
		{
			_savedQuote = null;
			_domainValidationException = null;
			AppDateTime.GetUtcNow = () => _dateCreated;
			_userContext = TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Manager | UserRole.Member);
		}

		#region Create

		[Test]
		public void Create_ValidQuoteDetails_QuoteCreated()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var orderNo = "PO1010";
			var adviceNo = "AD1010";

			var quoteRepositoryMock = MockRepository.GenerateMock<IQuoteRepository>();
			quoteRepositoryMock.Expect(x => x.Create(null)).IgnoreArguments();
			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				quoteRepositoryMock,
				QuoteServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				_userContext);
			CreateQuote(id, customerId, orderNo, adviceNo, currencyId);
			quoteRepositoryMock.VerifyAllExpectations();
			Assert.AreNotEqual(Guid.Empty, _savedQuote.Id);
			Assert.That(_savedQuote.QuoteNumber.StartsWith("QR"));
			Assert.AreEqual(_dateCreated, _savedQuote.DateCreated);
			Assert.AreEqual("graham.robertson@intertek.com", _savedQuote.CreatedBy.EmailAddress);
			Assert.AreEqual(1, _savedQuote.Revision);
			Assert.IsTrue(_savedQuote.IsActive);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidQuoteId_ArgumentExceptionThrown()
		{
			var id = Guid.Empty;
			var customerId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var orderNo = "PO1010";
			var adviceNo = "AD1010";

			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				_userContext);
			CreateQuote(id, customerId, orderNo, adviceNo, currencyId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidCustomerId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var orderNo = "PO1010";
			var adviceNo = "AD1010";

			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				QuoteServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsNull(customerId),
				MockRepository.GenerateStub<IListItemRepository>(),
				_userContext);
			CreateQuote(id, customerId, orderNo, adviceNo, currencyId);
		}

		[Test]
		public void Create_OrderNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var orderNo = new string('a', 51);
			var adviceNo = "AD1010";

			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				QuoteServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				_userContext);
			CreateQuote(id, customerId, orderNo, adviceNo, currencyId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Quotes.Messages.OrderNoTooLarge));
		}

		[Test]
		public void Create_AdviceNoGreaterThan50Characters_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = new string('a', 51);

			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				QuoteServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				_userContext);
			CreateQuote(id, customerId, orderNo, adviceNo, currencyId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Quotes.Messages.AdviceNoTooLarge));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidCurrencyId_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				QuoteServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNull(currencyId),
				_userContext);
			CreateQuote(id, customerId, orderNo, adviceNo, currencyId);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Create_InvalidListItemType_ArgumentExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "ad1111";

			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				QuoteServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsNonCurrencyListItem(currencyId),
				_userContext);
			CreateQuote(id, customerId, orderNo, adviceNo, currencyId);
		}

		[Test]
		public void Create_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			var id = Guid.NewGuid();
			var customerId = Guid.NewGuid();
			var currencyId = Guid.NewGuid();
			var orderNo = "PO1000";
			var adviceNo = "AD1000";

			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				QuoteServiceTestHelper.GetCustomerRepository_StubsGetById_ReturnsCustomer(customerId),
				ListItemRepositoryTestHelper.GetListItemRepository_StubsGetById_ReturnsGbpCurrency(currencyId),
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Member));
			CreateQuote(id, customerId, orderNo, adviceNo, currencyId);
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Quotes.Messages.InsufficientSecurityClearance));
		}

		private void CreateQuote(Guid id, Guid customerId, string orderNumber, string adviceNumber, Guid currencyId)
		{
			try
			{
				_savedQuote = _quoteService.Create(id, customerId, orderNumber, adviceNumber, currencyId);
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
			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			GetById(Guid.NewGuid());
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Quotes.Messages.InsufficientSecurity));
		}

		private void GetById(Guid id)
		{
			try
			{
				_quoteService.GetById(id);
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
		#region GetQuotes

		[Test]
		public void GetQuotes_UserHasInsufficientSecurityClearance_DomainValidationExceptionThrown()
		{
			_quoteService = QuoteServiceTestHelper.CreateQuoteService(
				MockRepository.GenerateStub<IQuoteRepository>(),
				MockRepository.GenerateStub<ICustomerRepository>(),
				MockRepository.GenerateStub<IListItemRepository>(),
				TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
			GetQuotes();
			Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Quotes.Messages.InsufficientSecurity));
		}

		private void GetQuotes()
		{
			try
			{
				_quoteService.GetQuotes();
			}
			catch (DomainValidationException dex)
			{
				_domainValidationException = dex;
			}
		}

		#endregion
	}
}