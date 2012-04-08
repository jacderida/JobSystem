using FluentNHibernate.Automapping.Alterations;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataAccess.NHibernate.Mappings
{
	public class CertificateMapping : IAutoMappingOverride<Certificate>
	{
		public void Override(FluentNHibernate.Automapping.AutoMapping<Certificate> mapping)
		{
			mapping.HasManyToMany(x => x.TestStandards).Cascade.All().Table("CertificatesTestStandards");
		}
	}
}