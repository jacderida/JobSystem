// <copyright company="Gael Limited">
// Copyright (c) 2010 All Right Reserved
// </copyright>
// <author></author>
// <email></email>
// <date>2010</date>
// <summary>
//	Complying with all copyright laws is the responsibility of the 
//	user. Without limiting rights under copyrights, neither the 
//	whole nor any part of this document may be reproduced, stored 
//	in or introduced into a retrieval system, or transmitted in any 
//	form or by any means (electronic, mechanical, photocopying, 
//	recording, or otherwise), or for any purpose without express 
//	written permission of Gael Limited.
// </summary>

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace JobSystem.BusinessLogic.Validation.Core
{
	/// <summary>
	/// A base validator for objects that have had their properties decorated with data annotations from .NET 4.
	/// </summary>
	/// <typeparam name="T">The type of the object to validate.</typeparam>
	internal abstract class AnnotatedObjectValidator<T> : IValidator<T>
	{
		/// <summary>
		/// Gets or sets the result of the validation.
		/// </summary>
		protected internal ValidationResult Result
		{
			get; set;
		}

		/// <summary>
		/// Validates an object of type T using the data annotations applied to any of its properties.
		/// This method makes a call to ValidateAgainstDomain, which performs any validation logic that
		/// cannot be performed using data annotations (any inherting classes are expected to provide an
		/// implementation for that abstract method, if necessary).
		/// </summary>
		/// <param name="obj">The object to validate.</param>
		/// <returns>The result of the validation.</returns>
		public ValidationResult Validate(T obj)
		{
			Result = new ValidationResult();

			var propInfos = typeof(T).GetProperties();
			for (var i = 0; i < propInfos.Length; i++)
			{
				try
				{
					var pi = (PropertyInfo)propInfos.GetValue(i);
					Validator.ValidateProperty(
						pi.GetValue(obj, new object[] { }),
						new ValidationContext(obj, null, null) { MemberName = pi.Name });
				}
				catch (ValidationException ex)
				{
					Result.Messages.Add(
						new ValidationMessage
						{
							Message = ex.ValidationResult.ErrorMessage,
							PropertyName = ex.ValidationResult.MemberNames.ToArray()[0]
						});
				}
			}
			ValidateAgainstDomain(obj);
			return Result;
		}

		/// <summary>
		/// Performs validation for an object of type T for any validation that cannot be performed using data annotations.
		/// </summary>
		/// <param name="obj">The object to validate.</param>
		protected abstract void ValidateAgainstDomain(T obj);
	}
}