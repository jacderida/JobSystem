using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate.Web;

namespace JobSystem.Mvc.Controllers
{
	public class EntityIdController : Controller
	{
		private readonly EntityIdProviderService _entityIdProviderService;

		public EntityIdController(EntityIdProviderService entityIdProviderService)
		{
			_entityIdProviderService = entityIdProviderService;
		}

		[HttpGet]
		[Transaction]
		public string GetId(string type)
		{
			return _entityIdProviderService.GetIdFor(type);
		}
	}
}