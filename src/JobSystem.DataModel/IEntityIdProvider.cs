namespace JobSystem.DataModel
{
	/// <summary>
	/// Generic interface for providing non Guid based IDs for entities of a particular type.
	/// </summary>
	public interface IEntityIdProvider
	{
		/// <summary>
		/// Gets an ID for use with an entity of a particular type.
		/// </summary>
		/// <typeparam name="T">The type of the entity for which to generate an ID.</typeparam>
		/// <returns>The ID as a string.</returns>
		string GetIdFor<T>();
	}
}