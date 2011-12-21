using System;
using System.Web.Mvc;

namespace JobSystem.DataAccess.NHibernate.Web
{
	/// <summary>
	/// Allows 'session per request'. Provided by Sharp Architecture.
	/// </summary>
	public class TransactionAttribute : ActionFilterAttribute
	{
		/// <summary>
		///     Optionally holds the factory key to be used when beginning/committing a transaction
		/// </summary>
		private readonly string _factoryKey;

		private bool _rollbackOnModelStateError = true;

		/// <summary>
		///     When used, assumes the <see cref = "_factoryKey" /> to be NHibernateSession.DefaultFactoryKey
		/// </summary>
		public TransactionAttribute()
		{
		}

		/// <summary>
		///     Overrides the default <see cref = "_factoryKey" /> with a specific factory key
		/// </summary>
		public TransactionAttribute(string factoryKey)
		{
			_factoryKey = factoryKey;
		}

		public bool RollbackOnModelStateError
		{
			get { return _rollbackOnModelStateError; }
			set { _rollbackOnModelStateError = value; }
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var effectiveFactoryKey = GetEffectiveFactoryKey();
			var currentTransaction = NHibernateSession.CurrentFor(effectiveFactoryKey).Transaction;
			if (currentTransaction.IsActive)
				if (((filterContext.Exception != null) && filterContext.ExceptionHandled) || ShouldRollback(filterContext))
					currentTransaction.Rollback();
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			NHibernateSession.CurrentFor(GetEffectiveFactoryKey()).BeginTransaction();
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			var effectiveFactoryKey = GetEffectiveFactoryKey();
			var currentTransaction = NHibernateSession.CurrentFor(effectiveFactoryKey).Transaction;
			base.OnResultExecuted(filterContext);
			try
			{
				if (currentTransaction.IsActive)
				{
					if (((filterContext.Exception != null) && (!filterContext.ExceptionHandled)) || ShouldRollback(filterContext))
						currentTransaction.Rollback();
					else
						currentTransaction.Commit();
				}
			}
			finally
			{
				currentTransaction.Dispose();
			}
		}

		private string GetEffectiveFactoryKey()
		{
			return String.IsNullOrEmpty(_factoryKey) ? NHibernateSession.DefaultFactoryKey : _factoryKey;
		}

		private bool ShouldRollback(ControllerContext filterContext)
		{
			return RollbackOnModelStateError && !filterContext.Controller.ViewData.ModelState.IsValid;
		}
	}
}