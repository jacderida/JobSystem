using FluentNHibernate.Automapping.Alterations;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataAccess.NHibernate.Mappings
{
    public class JobMapping : IAutoMappingOverride<Job>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<Job> mapping)
        {
            mapping.HasManyToMany(x => x.Attachments).Cascade.All().Table("AttachmentsJobs");
        }
    }
}