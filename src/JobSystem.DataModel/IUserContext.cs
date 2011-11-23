using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel
{
	/// <summary>
	/// Exposes contextual user information.
	/// </summary>
	public interface IUserContext
	{
		/// <summary>
		/// Get Current User
		/// </summary>
		/// <returns>The UserAccount of the current user</returns>
		UserAccount GetCurrentUser();
	}
}