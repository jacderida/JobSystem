using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel;
using JobSystem.Framework.Messaging;
using JobSystem.DataModel.Repositories;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Dto;

namespace JobSystem.BusinessLogic.Services
{
	public class CompanyDetailsService : ServiceBase
	{
		private readonly ICompanyDetailsRepository _companyDetailsRepository;

		public CompanyDetailsService(
			IUserContext applicationContext,
			ICompanyDetailsRepository companyDetailsRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_companyDetailsRepository = companyDetailsRepository;
		}

		public CompanyDetails Create(
			Guid id, string name, Address addressDetails,
			string telephone, string fax, string email,
			string www, string regNo, string vatRegNo,
			string termsAndConditions, Guid defaultCurrencyId, Guid defaultTaxCodeId,
			Guid defaultPaymentTermId, Guid defaultBankDetailsId, byte[] mainLogo)
		{
			throw new NotImplementedException();
		}
	}
}