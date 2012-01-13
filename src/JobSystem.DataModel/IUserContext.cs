using JobSystem.DataModel.Entities;

namespace JobSystem.DataModel
{
	public interface IUserContext
	{
		UserAccount GetCurrentUser();
	}
}