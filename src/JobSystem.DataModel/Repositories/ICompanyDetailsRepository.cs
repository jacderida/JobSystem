using System;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
    public interface ICompanyDetailsRepository : IReadWriteRepository<CompanyDetails, Guid>
    {
        void ChangeMainLogo(Guid companyId, byte[] mainLogo);
        CompanyDetails GetCompany();
        byte[] GetLogoBytes();
        bool ApplyAllPrices();
        void UpdateCompanyDetails(CompanyDetails companyDetails);
    }
}