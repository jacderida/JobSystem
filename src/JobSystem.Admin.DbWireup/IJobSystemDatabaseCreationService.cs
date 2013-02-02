using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Entities;

namespace JobSystem.Admin.DbWireup
{
    public interface IJobSystemDatabaseCreationService
    {
        void CreateDatabase(bool dropExisting = false);
        void CreateJobSystemSchemaFromMigrations(string migrationsDllPath);
        void InsertDefaultData(JobSystemDefaultData defaultData);
        void InsertCompanyDetails(CompanyDetails companyDetails);

        BankDetails GetBankDetails(Guid id);
        TaxCode GetTaxCode(Guid id);
        Currency GetCurrency(Guid id);
        ListItem GetPaymentTerm(Guid id);
    }
}