using System;
using System.Collections.Generic;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
    public class OrderRepositoryTestHelper
    {
        public static IOrderRepository GetOrderRepository_StubsGetById_ReturnsOrderWith0Items(Guid orderId)
        {
            var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
            orderRepositoryStub.Stub(x => x.GetById(orderId)).Return(GetOrder(orderId));
            return orderRepositoryStub;
        }

        public static IOrderRepository GetOrderRepository_StubsGetById_ReturnsOrderWith1Item(Guid orderId)
        {
            var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
            orderRepositoryStub.Stub(x => x.GetById(orderId)).Return(GetOrderWith1Item(orderId));
            return orderRepositoryStub;
        }

        public static IOrderRepository GetOrderRepository_StubsGetById_ReturnsNull(Guid orderId)
        {
            var orderRepositoryStub = MockRepository.GenerateStub<IOrderRepository>();
            orderRepositoryStub.Stub(x => x.GetById(orderId)).Return(null);
            return orderRepositoryStub;
        }

        public static Order GetOrder(Guid orderId)
        {
            return new Order
            {
                Id = orderId,
                CreatedBy = new UserAccount
                {
                    Id = Guid.NewGuid(),
                    EmailAddress = "chris.oneil@gmail.com",
                    Name = "Chris O'Neil",
                    JobTitle = "Development Engineer",
                    Roles = UserRole.Manager | UserRole.Member,
                    PasswordHash = "passwordhash",
                    PasswordSalt = "passwordsalt"
                },
                DateCreated = DateTime.UtcNow,
                Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" },
                Instructions = "blah blah",
                OrderNo = "OR2000",
                Currency = new Currency
                {
                    Id = Guid.NewGuid(),
                    Name = "Gbp",
                    DisplayMessage = "All prices in GBP"
                },
                OrderItems = new List<OrderItem>()
            };
        }

        public static Order GetOrderWith1Item(Guid orderId)
        {
            return new Order
            {
                Id = orderId,
                CreatedBy = new UserAccount
                {
                    Id = Guid.NewGuid(),
                    EmailAddress = "chris.oneil@gmail.com",
                    Name = "Chris O'Neil",
                    JobTitle = "Development Engineer",
                    Roles = UserRole.Manager | UserRole.Member,
                    PasswordHash = "passwordhash",
                    PasswordSalt = "passwordsalt"
                },
                DateCreated = DateTime.UtcNow,
                Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Gael Ltd" },
                Instructions = "blah blah",
                OrderNo = "OR2000",
                Currency = new Currency
                {
                    Id = Guid.NewGuid(),
                    Name = "Gbp",
                    DisplayMessage = "All prices in GBP"
                },
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ItemNo = 1,
                        Quantity = 1
                    }
                }
            };
        }
    }
}