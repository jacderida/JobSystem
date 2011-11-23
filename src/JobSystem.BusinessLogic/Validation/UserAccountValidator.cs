using System;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;

namespace JobSystem.BusinessLogic.Validation
{
	/// <summary>
	/// A utility class to demonstrate performing validation that can't be done with data annotations: to be removed.
	/// </summary>
	internal class UserAccountValidator : AnnotatedObjectValidator<UserAccount>
	{
		private readonly IUserAccountRepository _respository;

		public UserAccountValidator(IUserAccountRepository repository)
		{
			_respository = repository;
		}

		protected override void ValidateAgainstDomain(UserAccount obj)
		{
			CheckIfEmailExists(obj);
		}

		protected void CheckIfEmailExists(UserAccount obj)
		{
			// Validated by an attribute.
			if (String.IsNullOrEmpty(obj.EmailAddress))
				return;
			var user = _respository.GetByEmail(obj.EmailAddress, true);
			if (user != null && user.Id != obj.Id)
				Result.AddError(
					String.Format(JobSystem.Resources.UserAccounts.Messages.DuplicateEmailTemplate, obj.EmailAddress),
					"EmailAddress");
		}
	}
}