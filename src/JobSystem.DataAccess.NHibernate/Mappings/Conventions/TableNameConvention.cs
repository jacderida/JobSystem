using FluentNHibernate.Conventions;

namespace JobSystem.DataAccess.NHibernate.Mappings.Conventions
{
	/// <summary>
	/// The Table Name convention, which in this case is the Plural of the entity name e.g. User becomes Users
	/// </summary>
	public class TableNameConvention : IClassConvention
	{
		/// <summary>
		/// Applies the table conventions
		/// </summary>
		/// <param name="instance">the class instance to apply the convention on.</param>
		public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
		{
			instance.Table(Inflector.Net.Inflector.Pluralize(instance.EntityType.Name));
		}
	}
}