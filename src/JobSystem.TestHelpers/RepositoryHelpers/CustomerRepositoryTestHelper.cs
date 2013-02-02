using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
    public class CustomerRepositoryTestHelper
    {
        public static ICustomerRepository GetCustomerRepository_StubsGetById_ReturnsCustomer(Guid customerId)
        {
            var customerRepositoryStub = MockRepository.GenerateStub<ICustomerRepository>();
            customerRepositoryStub.Stub(x => x.GetById(customerId)).Return(GetCustomer(customerId));
            return customerRepositoryStub;
        }

        public static ICustomerRepository GetCustomerRepository_StubsGetById_ReturnsNull(Guid customerId)
        {
            var customerRepositoryStub = MockRepository.GenerateStub<ICustomerRepository>();
            customerRepositoryStub.Stub(x => x.GetById(customerId)).Return(null);
            return customerRepositoryStub;
        }

        private static Customer GetCustomer(Guid customerId)
        {
            return new Customer
            {
                Id = customerId,
                Name = "EMIS (UK) Ltd"
            };
        }
    }
}