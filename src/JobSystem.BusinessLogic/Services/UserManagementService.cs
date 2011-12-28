using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.BusinessLogic.Validation;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.BusinessLogic.Validation.Extensions;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Framework.Security;
using JobSystem.Resources.UserAccounts;

namespace JobSystem.BusinessLogic.Services
{
	public class UserManagementService : ServiceBase
	{
		private readonly UserAccountValidator _userAccountValidator;
		private readonly IUserAccountRepository _userAccountRepository;
		private readonly ICryptographicService _cryptographicService;

		public UserManagementService(
			IUserContext applicationContext,
			IUserAccountRepository userAccountRepository,
			ICryptographicService cryptographicService,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			Check.NotNull(userAccountRepository, "userAccountRepository cannot be null");
			_userAccountRepository = userAccountRepository;
			_cryptographicService = cryptographicService;
			_userAccountValidator = new UserAccountValidator(_userAccountRepository);
		}

		public UserAccount Create(Guid id, string name, string email, string password, string jobTitle, UserRole roles)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied to create a user");
			if (String.IsNullOrEmpty(password))
				throw new ArgumentException("A password must be supplied to create a user");
			if (!CurrentUser.HasRole(UserRole.Admin))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			var userAccount = new UserAccount
			{
				Id = id,
				EmailAddress = email,
				Name = name,
				JobTitle = jobTitle,
				Roles = roles
			};
			SetPassword(userAccount, password);
			ValidateAnnotatedObjectThrowOnFailure(userAccount);
			_userAccountValidator.ValidateThrowOnFailure(userAccount);
			_userAccountRepository.Create(userAccount);
			return userAccount;
		}

		public UserAccount Edit(Guid id, string name, string emailAddress, string jobTitle)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("An invalid ID was supplied for the user", "id");
			var userAccount = _userAccountRepository.GetById(id);
			if (userAccount == null)
				throw new ArgumentException("The user with specified ID does not exist", "id");
			if (!CurrentUser.HasRole(UserRole.Admin))
				throw new DomainValidationException(Messages.InsufficientSecurityClearanceEdit, "CurrentUser");
			userAccount.Name = name;
			userAccount.EmailAddress = emailAddress;
			userAccount.JobTitle = jobTitle;
			ValidateAnnotatedObjectThrowOnFailure(userAccount);
			_userAccountValidator.ValidateThrowOnFailure(userAccount);
			_userAccountRepository.Update(userAccount);
			return userAccount;
		}

		public UserAccount GetById(Guid id)
		{
			return _userAccountRepository.GetById(id);
		}

		public IList<UserAccount> GetUsers()
		{
			return _userAccountRepository.GetUsers().ToList();
		}

		public bool Login(string email, string password)
		{
			if (String.IsNullOrEmpty(email))
				throw new DomainValidationException(JobSystem.Resources.UserAccounts.Messages.LoginFailed);
			var user = _userAccountRepository.GetByEmail(email, false);
			if (user == null)
				throw new DomainValidationException(JobSystem.Resources.UserAccounts.Messages.LoginFailed);
			if (String.IsNullOrEmpty(password))
				throw new DomainValidationException(JobSystem.Resources.UserAccounts.Messages.LoginFailed);
			return ValidatePassword(user, password);
		}

		public void Logout()
		{
			// ??
		}

		private bool ValidatePassword(UserAccount userAccount, string password)
		{
			var hash = _cryptographicService.ComputeHash(password, userAccount.PasswordSalt);
			return hash == userAccount.PasswordHash;
		}

		private void SetPassword(UserAccount userAccount, string password)
		{
			userAccount.PasswordSalt = _cryptographicService.GenerateSalt();
			userAccount.PasswordHash = _cryptographicService.ComputeHash(password, userAccount.PasswordSalt);
		}
	}
}