using System;
using System.ComponentModel.DataAnnotations;
using JobSystem.Resources.UserAccounts;
using JobSystem.DataAnnotations;

namespace JobSystem.DataModel.Entities
{
	/// <summary>
	/// Represents an account for a user
	/// </summary>
	[Serializable]
	public class UserAccount
	{
		/// <summary>
		/// Anonymous User account. 
		/// </summary>
		public static readonly UserAccount Anonymous = new UserAccount
		{
			Id = Guid.Empty,
			Name = "Anonymous",
			EmailAddress = "anonymous@anonymous.com",
			JobTitle = "Anonymous"
		};

		public virtual Guid Id { get; set; }

		[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Messages))]
		[StringLength(255, MinimumLength = 1, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(Messages))]
		public virtual string Name { get; set; }

		[ReadableName("Email Address")]
		[StringLength(256, ErrorMessageResourceName = "EmailTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = (typeof(Messages)))]
		[EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Messages))]
		public virtual string EmailAddress { get; set; }

		[ReadableName("Job Title")]
		[StringLength(256, MinimumLength = 3, ErrorMessageResourceName = "JobTitleTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Required(ErrorMessageResourceName = "JobTitleRequired", ErrorMessageResourceType = (typeof(Messages)))]
		public virtual string JobTitle { get; set; }

		public UserRole Roles { get; set; }
		public virtual string PasswordSalt { get; set; }
		public virtual string PasswordHash { get; set; }

		public virtual bool HasRole(UserRole roles)
		{
			return (Roles & roles) != UserRole.None;
		}

		public override string ToString()
		{
			return EmailAddress;
		}

		public override bool Equals(object obj)
		{
			var user = obj as UserAccount;
			if (user == null)
				return false;
			return Id.Equals(user.Id) && EmailAddress.Equals(user.EmailAddress, StringComparison.CurrentCultureIgnoreCase);
		}

		public override int GetHashCode()
		{
			if (Id.Equals(Guid.Empty))
			{
				unchecked
				{
					int result = this.Name.GetHashCode();
					return (31 * result) ^ Id.GetHashCode();
				}
			}

			unchecked
			{
				int result = EmailAddress.GetHashCode();
				return (31 * result) ^ Id.GetHashCode();
			}
		}
	}
}