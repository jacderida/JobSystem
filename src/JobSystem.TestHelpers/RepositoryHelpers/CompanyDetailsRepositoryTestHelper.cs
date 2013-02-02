using JobSystem.DataModel.Repositories;
using Rhino.Mocks;

namespace JobSystem.TestHelpers.RepositoryHelpers
{
    public class CompanyDetailsRepositoryTestHelper
    {
        public static ICompanyDetailsRepository GetCompanyDetailsRepository_StubsApplyAllPrices_ReturnsFalse()
        {
            var companyDetailsRepository = MockRepository.GenerateStub<ICompanyDetailsRepository>();
            companyDetailsRepository.Stub(x => x.ApplyAllPrices()).Return(false);
            return companyDetailsRepository;
        }

        public static ICompanyDetailsRepository GetCompanyDetailsRepository_StubsApplyAllPrices_ReturnsTrue()
        {
            var companyDetailsRepository = MockRepository.GenerateStub<ICompanyDetailsRepository>();
            companyDetailsRepository.Stub(x => x.ApplyAllPrices()).Return(true);
            return companyDetailsRepository;
        }
    }
}