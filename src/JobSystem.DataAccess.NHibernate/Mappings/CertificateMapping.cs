using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobSystem.DataModel.Entities;
using FluentNHibernate.Automapping.Alterations;

namespace JobSystem.DataAccess.NHibernate.Mappings
{
	class CertificateMapping : IAutoMappingOverride<Certificate>
	{
		public void Override(FluentNHibernate.Automapping.AutoMapping<Certificate> mapping)
		{
			mapping.HasManyToMany(x => x.TestStandards).Cascade.All().Table("CertificatesTestStandards");
		}
	}
}