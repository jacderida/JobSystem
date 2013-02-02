using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
    public class SupplierRepositoryTestHelper
    {
        public static ISupplierRepository GetSupplierRepository_GetById_ReturnsSupplier(Guid supplierId)
        {
            var supplierRepository = MockRepository.GenerateStub<ISupplierRepository>();
            supplierRepository.Stub(x => x.GetById(supplierId)).Return(GetSupplier(supplierId));
            return supplierRepository;
        }

        public static ISupplierRepository GetSupplierRepository_GetById_ReturnsNull(Guid supplierId)
        {
            var supplierRepository = MockRepository.GenerateStub<ISupplierRepository>();
            supplierRepository.Stub(x => x.GetById(supplierId)).Return(null);
            return supplierRepository;
        }

        private static Supplier GetSupplier(Guid supplierId)
        {
            return new Supplier
            {
                Id = supplierId,
                Name = "Gael Ltd"
            };
        }
    }
}