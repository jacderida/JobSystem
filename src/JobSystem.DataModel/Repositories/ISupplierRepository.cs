using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
    public interface ISupplierRepository : IReadWriteRepository<Supplier, Guid>
    {
        Supplier GetByName(string name);
        int GetSuppliersCount();
        IEnumerable<Supplier> GetSuppliers();
        IEnumerable<Supplier> SearchByKeyword(string keyword);
    }
}