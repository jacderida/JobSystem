using FluentNHibernate.Automapping.Alterations;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataAccess.NHibernate.Mappings
{
    public class CompanyDetailsMapping : IAutoMappingOverride<CompanyDetails>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<CompanyDetails> mapping)
        {
            mapping.Map(x => x.MainLogo).CustomType("BinaryBlob").Length(int.MaxValue).Nullable();
        }
    }
}