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
		[StringLength(256, MinimumLength = 3, ErrorMessageResourceName = "EmailTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = (typeof(Messages)))]
		[EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Messages))]
		public virtual string EmailAddress { get; set; }

		[ReadableName("Job Title")]
		[StringLength(256, MinimumLength = 3, ErrorMessageResourceName = "JobTitleTooLarge", ErrorMessageResourceType = typeof(Messages))]
		[Required(ErrorMessageResourceName = "JobTitleRequired", ErrorMessageResourceType = (typeof(Messages)))]
		public virtual string JobTitle { get; set; }

		public virtual string PasswordSalt { get; set; }
		public virtual string PasswordHash { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="UserAccount"/> class.
		/// </summary>
		public UserAccount()
		{
		}

		/// <summary>
		/// Returns a <see cref="string"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="string"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return EmailAddress;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			var user = obj as UserAccount;
			if (user == null)
				return false;
			return Id.Equals(user.Id) && EmailAddress.Equals(user.EmailAddress, StringComparison.CurrentCultureIgnoreCase);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
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