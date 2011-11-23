using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using System;

namespace JobSystem.BusinessLogic.Validation
{
	internal class CustomerValidator : AnnotatedObjectValidator<Customer>
	{
		private ICustomerRepository _customerRepository;

		public CustomerValidator(ICustomerRepository repository)
		{
			_customerRepository = repository;
		}

		protected override void ValidateAgainstDomain(Customer obj)
		{
			CheckIfNameIsUnique(obj);
		}

		private void CheckIfNameIsUnique(Customer obj)
		{
			if (_customerRepository.GetByName(obj.Name) != null)
				Result.AddError(
					String.Format(JobSystem.Resources.Customers.Messages.DuplicateName, obj.Name),
					"Name");
		}
	}
}