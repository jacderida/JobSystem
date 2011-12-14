using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;

namespace JobSystem.BusinessLogic.Validation
{
	internal class CustomerValidator : IValidator<Customer>
	{
		private ICustomerRepository _customerRepository;

		public CustomerValidator(ICustomerRepository repository)
		{
			_customerRepository = repository;
		}

		public List<ValidationResult> Validate(Customer obj)
		{
			var result = new List<ValidationResult>();
			CheckIfNameIsUnique(result, obj);
			return result;
		}

		private void CheckIfNameIsUnique(List<ValidationResult> validationResult, Customer obj)
		{
			var customer = _customerRepository.GetByName(obj.Name);
			if (customer != null && customer.Id != obj.Id)
				validationResult.Add(new ValidationResult(String.Format(JobSystem.Resources.Customers.Messages.DuplicateName, obj.Name), new string[] { "Name" }));
		}
	}
}