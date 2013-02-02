using System;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.TaxCodes;

namespace JobSystem.BusinessLogic.Services
{
    public class TaxCodeService : ServiceBase
    {
        private readonly ITaxCodeRepository _taxCodeRepository;

        public TaxCodeService(
            IUserContext applicationContext,
            ITaxCodeRepository taxCodeRepository,
            IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
        {
            _taxCodeRepository = taxCodeRepository;
        }

        public TaxCode Create(Guid id, string taxCodeName, string description, double rate)
        {
            if (_taxCodeRepository.GetByName(taxCodeName) != null)
                throw new DomainValidationException(Messages.DuplicateName);
            var taxCode = new TaxCode();
            taxCode.Id = id;
            taxCode.TaxCodeName = taxCodeName;
            taxCode.Description = description;
            taxCode.Rate = rate;
            ValidateAnnotatedObjectThrowOnFailure(taxCode);
            _taxCodeRepository.Update(taxCode);
            return taxCode;
        }
    }
}