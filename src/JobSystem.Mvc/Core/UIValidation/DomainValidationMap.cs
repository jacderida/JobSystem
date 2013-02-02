using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Text;

namespace JobSystem.Mvc.Core.UIValidation
{
    /// <summary>
    /// A class that represents a collection of mappings from domain entity properties to view model properties.
    /// To use it, use a collection initializer.
    /// e.g. new DomainValidationMap&lt;T1 T2> { {d ()=> d.EMailAddress, v ()=> v.EmailField} }
    /// </summary>
    /// <typeparam name="TSourceDomainEntity">The type of the domain entity to map FROM.</typeparam>
    /// <typeparam name="TTargetViewModel">The type of the view model to map TO.</typeparam>
    public class DomainValidationMap<TSourceDomainEntity, TTargetViewModel> : List<Tuple<Expression<Func<TSourceDomainEntity, object>>, Expression<Func<TTargetViewModel, object>>>>
    {
        /// <summary>
        /// Adds the specified mapping.
        /// </summary>
        /// <param name="fromProperty">the property of the Domain Entity to map from in the format of a lambda expression.
        /// i.e. domainEntity => domainEntity.Property</param>
        /// <param name="toProperty">the property of the UI View Model to map to in the format of a lambda expression.
        /// i.e. viewModel => viewModel.Property</param>
        public void Add(Expression<Func<TSourceDomainEntity, object>> fromProperty, Expression<Func<TTargetViewModel, object>> toProperty)
        {
            Add(new Tuple<Expression<Func<TSourceDomainEntity, object>>, Expression<Func<TTargetViewModel, object>>>(fromProperty, toProperty));
        }

        /// <summary>
        /// Converts the current instance of the DomainValidationMap to a StringDictionary.
        /// </summary>
        /// <returns>A StringDictionary containing the source Domain Entities Property Names as the keys and the target View Entities Property Names as the values.</returns>
        public StringDictionary ToStringDictionary()
        {
            var dict = new StringDictionary();
            foreach (var mapping in this)
            {
                var fromProperty = GetPropertyName(mapping.Item1);
                var toProperty = GetPropertyName(mapping.Item2);

                if (!dict.ContainsKey(fromProperty))
                    dict.Add(fromProperty, toProperty);
            }
            return dict;
        }

        /// <summary>
        /// Gets the name of a property as a string
        /// </summary>
        /// <typeparam name="T">The type of the object to query.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression to evaluate to retrieve the propertyName.</param>
        /// <returns>the name of the appropriate property</returns>
        private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (ExpressionType.MemberAccess == expression.Body.NodeType)
            {
                var memberExpresssion = expression.Body as MemberExpression;
                if (memberExpresssion.Expression.NodeType == ExpressionType.Parameter)
                    return memberExpresssion.Member.Name;

                // Traverse the Expression tree until we get to the root object
                var stringBuilder = new StringBuilder();
                Expression exp = memberExpresssion;
                while (exp.NodeType == ExpressionType.MemberAccess)
                {
                    if (stringBuilder.Length != 0)
                        stringBuilder.Insert(0, ".");
                    var mex = exp as MemberExpression;
                    stringBuilder.Insert(0, mex.Member.Name);
                    exp = mex.Expression;
                }
                return stringBuilder.ToString();
            }

            if (ExpressionType.Call == expression.Body.NodeType)
                return (expression.Body as MethodCallExpression).Method.Name;
            if (ExpressionType.Convert == expression.Body.NodeType)
                return ((expression.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            throw new InvalidOperationException("Unsupported NodeType: '" + expression.Body.NodeType + "'");
        }
    }
}