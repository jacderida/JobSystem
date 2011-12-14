using System.Collections.Generic;
using JobSystem.BusinessLogic.Validation.Core;

namespace JobSystem.BusinessLogic.Validation.Extensions
{
	/// <summary>
	/// Validation extension methods.
	/// </summary>
	public static class ValidationExtensions
	{
		#region Methods

		/// <summary>
		/// Validates <paramref name="toValidate"/> using <paramref name="validator"/>, throwing a <see cref="DomainValidationException"/>
		/// if validation fails.
		/// </summary>
		/// <typeparam name="T">Type of entity to be validates</typeparam>
		/// <param name="validator">validator used to validate <paramref name="toValidate"/></param>
		/// <param name="toValidate">entity to be validated</param>
		public static void ValidateThrowOnFailure<T>(this IValidator<T> validator, T toValidate)
		{
			var result = validator.Validate(toValidate);
			if (result.Count > 0)
				throw new DomainValidationException(result);
		}
		
		/// <summary>
		/// Validates each item in <paramref name="toValidate"/> using <paramref name="validator"/>, throwing a <see cref="DomainValidationException"/>
		/// if validation fails
		/// </summary>
		/// <typeparam name="T">Type of entity to be validates</typeparam>
		/// <param name="validator">validator used to validate instances of <typeparamref name="T"/></param>
		/// <param name="toValidate">list of <typeparamref name="T"/> instances to be validated</param>
		public static void ValidateThrowOnFailure<T>(this IValidator<T> validator, IEnumerable<T> toValidate)
		{
			foreach (var item in toValidate)
				validator.ValidateThrowOnFailure(item);
		}

		#endregion
	}
}