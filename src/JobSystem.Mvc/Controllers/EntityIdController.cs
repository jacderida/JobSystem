using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataModel;
using System;

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
		public string GetId(string id)
		{
			var entityId = String.Empty;
			try
			{
				NHibernateSession.Current.BeginTransaction();
				entityId = _entityIdProviderService.GetIdFor(id);
				NHibernateSession.TryCommitTransactionForEntityWithUniqueId();
			}
			catch (EntityIdNotUniqueException)
			{
				EntityIdNotUniqueException notUniqueEx = null;
				do
				{
					try
					{
						NHibernateSession.Current.BeginTransaction();
						entityId = _entityIdProviderService.GetIdFor(id);
						NHibernateSession.TryCommitTransactionForEntityWithUniqueId();
						notUniqueEx = null;
					}
					catch (EntityIdNotUniqueException ex)
					{
						notUniqueEx = ex;
					}
				} while (notUniqueEx != null);
			}
			finally
			{
				NHibernateSession.Current.Transaction.Dispose();
			}
			return entityId;
		}
	}
}