using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;

namespace JobSystem.TestHelpers.Context
{
    public class TestUserContext : IUserContext
    {
        private readonly UserAccount _userAccount;

        public TestUserContext()
            : this(UserAccount.Anonymous)
        {
        }

        public TestUserContext(UserAccount userAccount)
        {
            _userAccount = userAccount;
        }

        public TestUserContext(Guid userId, string emailAddress, string userName, string jobTitle, UserRole roles)
            : this(new UserAccount()
                            {
                                Id = userId,
                                Name = userName,
                                JobTitle = jobTitle,
                                EmailAddress = emailAddress,
                                Roles = roles
                            })
        {
        }

        public static IUserContext Create(string emailAddress, string userName, string jobTitle, UserRole roles)
        {
            return new TestUserContext(Guid.NewGuid(), emailAddress, userName, jobTitle, roles);
        }

        public static UserAccount CreateAdminUser()
        {
            return new UserAccount
            {
                Id = Guid.NewGuid(),
                EmailAddress = "admin@intertek.com",
                Name = "Graham Robertson",
                JobTitle = "Laboratory Manager",
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
        }

        public UserAccount GetCurrentUser()
        {
            return _userAccount;
        }
    }
}