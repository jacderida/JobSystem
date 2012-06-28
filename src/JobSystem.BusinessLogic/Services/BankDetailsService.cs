using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;

namespace JobSystem.BusinessLogic.Services
{
	public class BankDetailsService : ServiceBase
	{
		private readonly IBankDetailsRepository _bankDetailsRepository;

		public BankDetailsService(
			IUserContext applicationContext,
			IBankDetailsRepository bankDetailsRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_bankDetailsRepository = bankDetailsRepository;
		}

		public BankDetails Create(
			Guid id, string name, string shortName, string accountNo, string sortCode, string address1, string address2, string address3, string address4, string address5, string iban)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the bank account");
			var bankDetails = new BankDetails();
			bankDetails.Id = id;
			bankDetails.Name = name;
			bankDetails.ShortName = shortName;
			bankDetails.AccountNo = accountNo;
			bankDetails.SortCode = sortCode;
			bankDetails.Address1 = address1;
			bankDetails.Address2 = address2;
			bankDetails.Address3 = address3;
			bankDetails.Address4 = address4;
			bankDetails.Address5 = address5;
			bankDetails.Iban = iban;
			ValidateAnnotatedObjectThrowOnFailure(bankDetails);
			_bankDetailsRepository.Create(bankDetails);
			return bankDetails;
		}
	}
}