using System;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
    public static class ListItemRepositoryTestHelper
    {
        public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsPaymentTerm(Guid paymentTermId)
        {
            var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
            if (paymentTermId != Guid.Empty)
                listItemRepository.Stub(x => x.GetById(paymentTermId)).Return(GetPaymentTerm(paymentTermId));
            else
                listItemRepository.Stub(x => x.GetById(paymentTermId)).Return(null);
            return listItemRepository;
        }

        public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNonPaymentTerm(Guid paymentTermId)
        {
            var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
            listItemRepository.Stub(x => x.GetById(paymentTermId)).Return(GetHouseCalibrationCertificateType(paymentTermId));
            return listItemRepository;
        }

        public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNull(Guid itemId)
        {
            var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
            listItemRepository.Stub(x => x.GetById(itemId)).Return(null);
            return listItemRepository;
        }

        public static IListItemRepository GetListItemRepository_StubsGetByType_ReturnsListItem(ListItemType[] types)
        {
            var listItemRepositoryStub = MockRepository.GenerateStub<IListItemRepository>();
            foreach (var type in types)
                listItemRepositoryStub.Stub(x => x.GetByType(type)).Return(GetByType(type));
            return listItemRepositoryStub;
        }

        public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsCertificateAndCategory(Guid certificateTypeId, Guid categoryId)
        {
            var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
            listItemRepository.Stub(x => x.GetById(certificateTypeId)).Return(GetHouseCalibrationCertificateType(certificateTypeId));
            listItemRepository.Stub(x => x.GetById(categoryId)).Return(GetCertificateCategory(categoryId));
            return listItemRepository;
        }

        public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNullForCertificate(Guid certificateTypeId, Guid categoryId)
        {
            var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
            listItemRepository.Stub(x => x.GetById(certificateTypeId)).Return(null);
            listItemRepository.Stub(x => x.GetById(categoryId)).Return(GetCertificateCategory(categoryId));
            return listItemRepository;
        }

        public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNullForCategory(Guid certificateTypeId, Guid categoryId)
        {
            var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
            listItemRepository.Stub(x => x.GetById(certificateTypeId)).Return(GetHouseCalibrationCertificateType(certificateTypeId));
            listItemRepository.Stub(x => x.GetById(categoryId)).Return(null);
            return listItemRepository;
        }

        public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNonCertificateType(Guid certificateTypeId)
        {
            var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
            listItemRepository.Stub(x => x.GetById(certificateTypeId)).Return(GetPaymentTerm(certificateTypeId));
            return listItemRepository;
        }

        public static IListItemRepository GetListItemRepository_StubsGetById_ReturnsNonCategoryType(Guid certificateTypeId, Guid categoryId)
        {
            var listItemRepository = MockRepository.GenerateStub<IListItemRepository>();
            listItemRepository.Stub(x => x.GetById(certificateTypeId)).Return(GetHouseCalibrationCertificateType(certificateTypeId));
            listItemRepository.Stub(x => x.GetById(categoryId)).Return(GetPaymentTerm(categoryId));
            return listItemRepository;
        }

        private static ListItem GetByType(ListItemType type)
        {
            return new ListItem
            {
                Id = Guid.NewGuid(),
                Name = type.ToString(),
                Type = type
            };
        }

        private static ListItem GetHouseCalibrationCertificateType(Guid certificateTypeId)
        {
            return new ListItem
            {
                Id = certificateTypeId,
                Name = "House",
                Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Certificate Type", Type = ListItemCategoryType.Certificate },
                Type = ListItemType.CertificateTypeHouse
            };
        }

        private static ListItem GetPaymentTerm(Guid paymentTermId)
        {
            return new ListItem
            {
                Id = paymentTermId,
                Name = "30 days",
                Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Certificate Category", Type = ListItemCategoryType.PaymentTerm },
                Type = ListItemType.PaymentTerm30Days
            };
        }

        private static ListItem GetCertificateCategory(Guid categoryId)
        {
            return new ListItem
            {
                Id = categoryId,
                Name = "D - Density",
                Category = new ListItemCategory { Id = Guid.NewGuid(), Name = "Certificate Category", Type = ListItemCategoryType.JobItemCategory },
                Type = ListItemType.CategoryDensity
            };
        }
    }
}