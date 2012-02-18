using System;
using System.Linq;
using System.Linq.Expressions;
using JobSystem.DataModel.Queries;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	public abstract class RepositoryBase<T> : RepositoryBase<T, Guid>
		where T : class
	{
		public RepositoryBase()
		{
		}
	}

	public abstract class RepositoryBase<T, TId>
		where T : class
	{
		protected ISession CurrentSession
		{
			get
			{
				return NHibernateSession.Current;
			}
		}

		public virtual bool Exists(TId id)
		{
			return CurrentSession.CreateCriteria<T>().Add(Restrictions.IdEq(id)).SetProjection(Projections.RowCount()).UniqueResult<int>() != 0;
		}

		public virtual T GetById(TId id)
		{
			return CurrentSession.Get<T>(id);
		}

		public virtual void Create(T entity)
		{
			CurrentSession.Save(entity);
		}

		public virtual IQueryable<T> FindAll(IQuery<T> query)
		{
			return query.Matches(CurrentSession.Query<T>());
		}

		public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> expression)
		{
			return FindAll(new LinqQuery<T>(expression));
		}

		public virtual void Update(T entity)
		{
			CurrentSession.Update(entity);
		}
	}
}