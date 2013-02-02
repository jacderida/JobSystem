using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Resources.Suppliers;

namespace JobSystem.BusinessLogic.Validation
{
    public class SupplierValidator : IValidator<Supplier>
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierValidator(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public List<ValidationResult> Validate(Supplier obj)
        {
            var result = new List<ValidationResult>();
            CheckIfNameIsUnique(result, obj);
            return result;
        }

        private void CheckIfNameIsUnique(List<ValidationResult> validationResult, Supplier obj)
        {
            var supplier = _supplierRepository.GetByName(obj.Name);
            if (supplier != null && supplier.Id != obj.Id)
                validationResult.Add(new ValidationResult(String.Format(Messages.DuplicateName, obj.Name), new string[] { "Name" }));
        }
    }
}