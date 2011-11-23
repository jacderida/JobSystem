using System.Collections.Specialized;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Validation.Core;

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
		public static void UpdateFromDomain(this ModelStateDictionary currentState, ValidationResult result)
		{
			foreach (var message in result.Messages)
				currentState.AddModelError(message.PropertyName, message.Message);
		}
		
		/// <summary>
		/// Adds any errors from the domain to the view model.
		/// </summary>
		/// <param name="currentState">The model state dictionary to update.</param>
		/// <param name="result">The result from the domain validation, which contains any errors that occurred in the domain validation.</param>
		/// <param name="mappingFromDomainToUi">Provides a set of mappings that map the propeties of a domain object to those of a view model object.</param>
		public static void UpdateFromDomain(this ModelStateDictionary currentState, ValidationResult result, StringDictionary mappingFromDomainToUi)
		{
			foreach (var message in result.Messages)
			{
				var propertyName = message.PropertyName;
				if (mappingFromDomainToUi.ContainsKey(message.PropertyName))
					propertyName = mappingFromDomainToUi[message.PropertyName];
				currentState.AddModelError(propertyName, message.Message);
			}
		}
	}
}