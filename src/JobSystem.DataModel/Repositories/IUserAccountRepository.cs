using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
    public interface IUserAccountRepository : IReadWriteRepository<UserAccount, Guid>
    {
        IList<UserAccount> GetUsers();
        UserAccount GetByEmail(string emailAddress, bool readOnly);
    }
}