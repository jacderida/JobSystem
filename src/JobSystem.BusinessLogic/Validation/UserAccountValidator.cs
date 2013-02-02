using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;

namespace JobSystem.BusinessLogic.Validation
{
    /// <summary>
    /// A utility class to demonstrate performing validation that can't be done with data annotations: to be removed.
    /// </summary>
    internal class UserAccountValidator : IValidator<UserAccount>
    {
        private readonly IUserAccountRepository _respository;

        public UserAccountValidator(IUserAccountRepository repository)
        {
            _respository = repository;
        }
        public List<ValidationResult> Validate(UserAccount obj)
        {
            var result = new List<ValidationResult>();
            CheckIfEmailExists(result, obj);
            return result;
        }

        private void CheckIfEmailExists(List<ValidationResult> validationResult, UserAccount obj)
        {
            if (String.IsNullOrEmpty(obj.EmailAddress))
                return;
            var user = _respository.GetByEmail(obj.EmailAddress, true);
            if (user != null && user.Id != obj.Id)
                validationResult.Add(new ValidationResult(String.Format(JobSystem.Resources.UserAccounts.Messages.DuplicateEmailTemplate, obj.EmailAddress), new string[] { "EmailAddress" }));
        }
    }
}