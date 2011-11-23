using System.Web;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;

namespace JobSystem.Mvc.Core
{
	/// <summary>
	/// Stores information about the current user.
	/// </summary>
	public class WebUserContext : IUserContext
	{
		private const string CurrentUserKey = "current.user";
		private readonly IUserAccountRepository _repository;
		private readonly HttpContextBase _httpContext;
		/// <summary>
		/// Initializes a new instance of the <see cref="WebUserContext"/> class.
		/// </summary>
		/// <param name="repository">The user account repository used to fetch the account</param>
		/// <param name="httpContext">The HTTP context.</param>
		public WebUserContext(IUserAccountRepository repository, HttpContextBase httpContext)
		{
			_httpContext = httpContext;
			_repository = repository;
		}

		#region IUserContext Members

		/// <summary>
		/// The user currently logged in. 
		/// </summary>
		/// <returns>The user account of the user currently logged ins</returns>
		public UserAccount GetCurrentUser()
		{
			UserAccount user = CurrentUser;
			if (user == null)
			{
				user = GetUserAccountFromPrincipal();
				if (user != UserAccount.Anonymous)
					CurrentUser = user;
			}
			return user;
		}

		#endregion

		private UserAccount GetUserAccountFromPrincipal()
		{
			var principal = _httpContext.User;
			if (principal == null || !principal.Identity.IsAuthenticated)
				return UserAccount.Anonymous;
			return _repository.GetByEmail(principal.Identity.Name, false) ?? UserAccount.Anonymous;
		}

		private UserAccount CurrentUser
		{
			get { return _httpContext.Items[CurrentUserKey] as UserAccount; }
			set { _httpContext.Items[CurrentUserKey] = value; }
		}
	}
}