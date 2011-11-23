using FluentNHibernate.Conventions;

namespace JobSystem.DataAccess.NHibernate.Mappings.Conventions
{
	/// <summary>
	/// The Primary Key convention, which in this case is a self assigned Guid.
	/// </summary>
	public class PrimaryKeyConvention : IIdConvention
	{
		/// <summary>
		/// Applies the convention
		/// </summary>
		/// <param name="instance">The instance to apply the convention</param>
		public void Apply(FluentNHibernate.Conventions.Instances.IIdentityInstance instance)
		{
			instance.Column("Id");
			instance.GeneratedBy.Assigned();
		}
	}
}