using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace JobSystem.Mvc.Core.UIValidation
{
    /// <summary>
    /// Extension methods to update a view model based on validation performed in the domain. 
    /// </summary>
    public static class ModelStateDomainValidation
    {
        /// <summary>
        /// Adds any errors from the domain to the view model.
        /// </summary>
        /// <param name="currentState">The model state dictionary to update.</param>
        /// <param name="result">The result from the domain validation, which contains any errors that occurred in the domain validation.</param>
        public static void UpdateFromDomain(this ModelStateDictionary currentState, List<ValidationResult> results)
        {
            foreach (var result in results)
                currentState.AddModelError(result.MemberNames.ToArray()[0], result.ErrorMessage);
        }
        
        /// <summary>
        /// Adds any errors from the domain to the view model.
        /// </summary>
        /// <param name="currentState">The model state dictionary to update.</param>
        /// <param name="result">The result from the domain validation, which contains any errors that occurred in the domain validation.</param>
        /// <param name="mappingFromDomainToUi">Provides a set of mappings that map the propeties of a domain object to those of a view model object.</param>
        public static void UpdateFromDomain(this ModelStateDictionary currentState, List<ValidationResult> results, StringDictionary mappingFromDomainToUi)
        {
            foreach (var result in results)
            {
                var propertyName = result.MemberNames.ToArray()[0];
                if (mappingFromDomainToUi.ContainsKey(propertyName))
                    propertyName = mappingFromDomainToUi[propertyName];
                currentState.AddModelError(propertyName, result.ErrorMessage);
            }
        }
    }
}