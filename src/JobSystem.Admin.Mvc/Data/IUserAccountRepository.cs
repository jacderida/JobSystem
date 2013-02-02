
namespace JobSystem.Admin.Mvc.Data
{
    public interface IUserAccountRepository
    {
        bool Login(string username, string password);
    }
}