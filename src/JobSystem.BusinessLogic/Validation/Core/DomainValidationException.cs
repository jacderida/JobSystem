using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JobSystem.BusinessLogic.Validation.Core
{
	/// <summary>
	/// The exception that is thrown when a domain object is in an invalid state.
	/// </summary>
	[Serializable]
	public class DomainValidationException : Exception
	{
		/// <summary>
		/// Gets or sets the result of the attempted validation, which contains a string based message to indicate the invalidity of the object.
		/// </summary>
		public ValidationResult Result { get; set; }
		
		/// <summary>
		/// Initialises an instance of the DomainValidationException class.
		/// The default constructor needs to be defined explicitly now since it would be gone otherwise.
		/// </summary>
		public DomainValidationException()
		{
		}

		/// <summary>
		/// Initialises an instance of the DomainValidationException class.
		/// </summary>
		/// <param name="message">The error message string.</param>
		public DomainValidationException(string message)
			: base(message)
		{
			Result = new ValidationResult();
			Result.AddError(message);
		}

		/// <summary>
		/// Initialises an instance of the DomainValidationException class.
		/// </summary>
		/// <param name="message">The error message string.</param>
		/// <param name="propertyName">The property that the error message relates to.</param>
		public DomainValidationException(string message, string propertyName)
		{
			Result = new ValidationResult();
			Result.AddError(message, propertyName);
		}

		/// <summary>
		/// Initialises an instance of the DomainValidationException class.
		/// </summary>
		/// <param name="message">The error message string.</param>
		/// <param name="innerException">The inner exception.</param>
		public DomainValidationException(string message, Exception innerException)
			: base(message, innerException)
		{
			Result = new ValidationResult();
			Result.AddError(message);
		}

		/// <summary>
		/// Initialises an instance of the DomainValidationException class.
		/// </summary>
		/// <param name="result">The validation result.</param>
		public DomainValidationException(ValidationResult result)
		{
			Result = result;
		}

		/// <summary>
		/// Sets the SerializationInfo with information about the exception.
		/// </summary>
		/// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Result", Result);
		}
	}
}