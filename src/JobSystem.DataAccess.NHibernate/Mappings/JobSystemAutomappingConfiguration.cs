using System;
using System.Linq;
using FluentNHibernate.Automapping;

namespace JobSystem.DataAccess.NHibernate.Mappings
{
    public class JobSystemAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        private static readonly string[] EntityNamespaces;

        static JobSystemAutomappingConfiguration()
        {
            EntityNamespaces = new string[]
            {
                "JobSystem.DataModel.Entities",
            };
        }

        public override bool IsId(FluentNHibernate.Member member)
        {
            return member.Name == "Id";
        }

        public override bool ShouldMap(Type type)
        {
            /* I've got NO IDEA why I have to add the manual check here for the enum type.
             * I have it setup with the conventions, exactly the way it's advised on stack overflow, and exactly the way that it's
             * setup with Alea. No difference from what I can see, yet the automapper is insisting on trying to map that type. */
            return EntityNamespaces.Contains(type.Namespace) && !type.IsEnum;
        }

        public override string GetComponentColumnPrefix(FluentNHibernate.Member member)
        {
            return String.Empty;
        }
    }
}