using System;
using System.Linq;
using System.Linq.Expressions;
using JobSystem.DataModel.Queries;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace JobSystem.DataAccess.NHibernate.Repositories
{
	/// <summary>
	/// A base that assumes the id is of type Guid
	/// </summary>
	/// <typeparam name="T">The Type of entity</typeparam>
	public abstract class RepositoryBase<T> : RepositoryBase<T, Guid>
		where T : class
	{
		public RepositoryBase()
		{
		}
	}

	/// <summary>
	/// A base class for all repositories
	/// </summary>
	/// <typeparam name="T">The type of entity</typeparam>
	/// <typeparam name="TId">The type of the identity field</typeparam>
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

		/// <summary>
		/// Check whether an entity with the given id exists
		/// </summary>
		/// <param name="id">the id to search for.</param>
		/// <returns>True if exists, false otherwise</returns>
		public virtual bool Exists(TId id)
		{
			return CurrentSession.CreateCriteria<T>().Add(Restrictions.IdEq(id)).SetProjection(Projections.RowCount()).UniqueResult<int>() != 0;
		}

		/// <summary>
		/// Gets an entity by it's unique id
		/// </summary>
		/// <param name="id">The id of the entity</param>
		/// <returns>The entity associated with the given id</returns>
		public virtual T GetById(TId id)
		{
			return CurrentSession.Get<T>(id);
		}

		/// <summary>
		/// Adds a new entity to the database
		/// </summary>
		/// <param name="entity">The entity to add</param>
		public virtual void Create(T entity)
		{
			CurrentSession.Save(entity);
		}

		/// <summary>
		/// Find all entities that meet the criteria of the given query 
		/// </summary>
		/// <param name="query">The pre-defined query user to filter the results</param>
		/// <returns>A list of all the entities that meet the criteria of the query</returns>
		public virtual IQueryable<T> FindAll(IQuery<T> query)
		{
			return query.Matches(CurrentSession.Query<T>());
		}

		/// <summary>
		/// Finds all entities that match the criteria of the expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns>a list of all the entities that meet the criteria of the expression</returns>
		public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> expression)
		{
			return FindAll(new LinqQuery<T>(expression));
		}

		/// <summary>
		/// Updates an existing entity
		/// </summary>
		/// <param name="entity">The entity to update</param>
		public virtual void Update(T entity)
		{
			CurrentSession.Update(entity);
		}
	}
}