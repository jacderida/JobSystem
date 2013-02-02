using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Helpers;
using JobSystem.DataAccess.NHibernate.Mappings.Conventions;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataAccess.NHibernate.Mappings
{
    public class AutoPersistenceModelGenerator
    {
        public AutoPersistenceModel Generate()
        {
            return AutoMap.AssemblyOf<UserAccount>(new JobSystemAutomappingConfiguration())
                .Conventions
                .Setup(GetConventions())
                .UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
        }

        private static Action<FluentNHibernate.Conventions.IConventionFinder> GetConventions()
        {
            return mappings =>
            {
                mappings.Add<PrimaryKeyConvention>();
                mappings.Add<TableNameConvention>();
                mappings.Add<EnumConvention>();
                mappings.Add(ForeignKey.EndsWith("Id"));
                mappings.Add(DefaultCascade.None());
                mappings.Add(DefaultAccess.Property());
                mappings.Add(DefaultLazy.Always());
                mappings.Add(LazyLoad.Always());
            };
        }
    }
}