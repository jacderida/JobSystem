using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel.Repositories
{
	/// <summary>
	/// Repository for looking up information regarding the next entity id to assign
	/// </summary>
	public interface IEntityIdLookupRepository
	{
		/// <summary>
		/// Gets information regarding the next entity id of the specified type.
		/// </summary>
		/// <param name="typeName">The type of entity.</param>
		/// <returns>information about the next entity id to be assigned.</returns>
		EntityIdLookup GetLookup(string typeName);

		/// <summary>
		/// Saves or updates the information about the next entity id.
		/// </summary>
		/// <param name="lookupId">The information about the next entity id.</param>
		void SaveOrUpdate(EntityIdLookup lookupId);

		/// <summary>
		/// Gets the next id for the particular type.
		/// </summary>
		/// <param name="typeName">the name of the type.</param>
		/// <returns>The next ID in the available sequence.</returns>
		int GetNextId(string typeName);
	}
}