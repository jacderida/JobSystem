using System;
using System.Linq;
using System.Linq.Expressions;

namespace JobSystem.DataModel.Queries
{
    /// <summary>
    /// Linq implemenation of the <see cref="IQuery"/> interface.
    /// </summary>
    /// <typeparam name="T">The entity that we are performing the search on</typeparam>
    public class LinqQuery<T> : IQuery<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        /// <summary>
        /// Construct an instance <see cref="AdHocQuery"/>
        /// </summary>
        public LinqQuery()
        {
        }

        /// <summary>
        /// Construct an instance <see cref="AdHocQuery"/>
        /// </summary>
        /// <param name="expression">The expression.</param>
        public LinqQuery(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Return the LINQ expression used to filter the results
        /// </summary>
        /// <returns> the LINQ expression</returns>
        public virtual Expression<Func<T, bool>> GetCriteria()
        {
            return _expression;
        }

        #region IQuery<T> Members

        /// <summary>
        /// Implementation of Matches
        /// </summary>
        /// <param name="values">The list of values to filter</param>
        /// <returns>The filtered results</returns>
        public virtual IQueryable<T> Matches(IQueryable<T> values)
        {
            var criteria = GetCriteria();
            if (criteria != null)
            {
                var debug = values.Where(criteria);
                return debug;
            }
            return values;
        }

        #endregion

        /// <summary>
        /// Methods that allows one to perform an and operation on two 
        /// queries.  
        /// </summary>
        /// <param name="expression">the expression to and with</param>
        /// <returns>A new query </returns>
        public LinqQuery<T> And(Expression<Func<T, bool>> expression)
        {
            return And(new LinqQuery<T>(expression));
        }

        /// <summary>
        /// Methods that allows one to perform an and operation on two 
        /// queries.  
        /// </summary>
        /// <param name="query">the expression to and with</param>
        /// <returns>A new query </returns>
        public LinqQuery<T> And(LinqQuery<T> query)
        {
            var leftQuery = GetCriteria();
            var rightQuery = query.GetCriteria();
            if (leftQuery == null)
                return query;
            if (rightQuery == null)
                return this;
            var invokedExpr = Expression.Invoke(rightQuery, leftQuery.Parameters);
            return new LinqQuery<T>(Expression.Lambda<Func<T, bool>>(Expression.AndAlso(leftQuery.Body, invokedExpr), rightQuery.Parameters));
        }

        /// <summary>
        /// Methods that allows one to perform an and operation on two 
        /// queries.  
        /// </summary>
        /// <param name="expression">the expression to and with</param>
        /// <returns>A new query </returns>
        public LinqQuery<T> Or(Expression<Func<T, bool>> expression)
        {
            return Or(new LinqQuery<T>(expression));
        }

        /// <summary>
        /// Methods that allows one to perform an and operation on two 
        /// queries.  
        /// </summary>
        /// <param name="query">the expression to and with</param>
        /// <returns>A new query </returns>
        public LinqQuery<T> Or(LinqQuery<T> query)
        {
            var leftQuery = GetCriteria();
            var rightQuery = query.GetCriteria();
            if (leftQuery == null)
                return query;
            if (rightQuery == null)
                return this;
            var invokedExpr = Expression.Invoke(rightQuery, leftQuery.Parameters);
            return new LinqQuery<T>(Expression.Lambda<Func<T, bool>>(Expression.OrElse(leftQuery.Body, invokedExpr), rightQuery.Parameters));
        }

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="lhs">The left hand side of the expression.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>The result of the operator.</returns>
        public static LinqQuery<T> operator &(LinqQuery<T> lhs, Expression<Func<T, bool>> expression)
        {
            if (lhs == null) throw new ArgumentNullException("lhs");
            if (expression == null) throw new ArgumentNullException("expression");
            return lhs.And(expression);
        }

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="lhs">The left hand side of the expression.</param>
        /// <param name="rhs">The right hand side of the expression.</param>
        /// <returns>The result of the operator.</returns>
        public static LinqQuery<T> operator &(LinqQuery<T> lhs, LinqQuery<T> rhs)
        {
            if (lhs == null) throw new ArgumentNullException("lhs");
            if (rhs == null) throw new ArgumentNullException("rhs");
            return lhs.And(rhs);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="lhs">The left hand side of the expression.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>The result of the operator.</returns>
        public static LinqQuery<T> operator |(LinqQuery<T> lhs, Expression<Func<T, bool>> expression)
        {
            if (lhs == null) throw new ArgumentNullException("lhs");
            if (expression == null) throw new ArgumentNullException("expression");
            return lhs.Or(expression);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="lhs">The left hand side of the expression.</param>
        /// <param name="rhs">The right hand side of the expression.</param>
        /// <returns>The result of the operator.</returns>
        public static LinqQuery<T> operator |(LinqQuery<T> lhs, LinqQuery<T> rhs)
        {
            if (lhs == null) throw new ArgumentNullException("lhs");
            if (rhs == null) throw new ArgumentNullException("rhs");
            return lhs.Or(rhs);
        }
    }
}