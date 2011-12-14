
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace JobSystem.BusinessLogic.Validation.Core
{
	[Serializable]
	public class DomainValidationException : Exception
	{
		public List<ValidationResult> Result { get; set; }
		
		public DomainValidationException()
		{
		}

		public DomainValidationException(string message)
			: base(message)
		{
			Result = new List<ValidationResult> { new ValidationResult(message) };
		}

		public DomainValidationException(string message, string propertyName)
		{
			Result = new List<ValidationResult> { new ValidationResult(message, new string[] { propertyName }) };
		}

		public DomainValidationException(string message, Exception innerException)
			: base(message, innerException)
		{
			Result = new List<ValidationResult> { new ValidationResult(message) };
		}

		public DomainValidationException(ValidationResult result)
		{
			Result = new List<ValidationResult> { result };
		}

		public DomainValidationException(List<ValidationResult> result)
		{
			Result = result;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Result", Result);
		}
	}
}