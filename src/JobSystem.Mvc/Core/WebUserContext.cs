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

        public WebUserContext(IUserAccountRepository repository, HttpContextBase httpContext)
        {
            _httpContext = httpContext;
            _repository = repository;
        }

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