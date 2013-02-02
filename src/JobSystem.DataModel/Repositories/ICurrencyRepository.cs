using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
    public interface ICurrencyRepository : IReadWriteRepository<Currency, Guid>
    {
        IEnumerable<Currency> GetCurrencies();
    }
}