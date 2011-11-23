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

using System.Collections.Generic;

namespace JobSystem.BusinessLogic.Validation.Core
{
	/// <summary>
	/// The result of a validation.
	/// </summary>
	public class ValidationResult
	{
		private bool _dirty = true;
		private bool _valid;

		/// <summary>
		/// Initialises an instance of the ValidationResult class.
		/// </summary>
		public ValidationResult()
		{
			Messages = new List<ValidationMessage>();
		}

		/// <summary>
		/// Gets a value indicating whether the result was valid.
		/// </summary>
		public bool IsValid
		{
			get
			{
				if (_dirty)
				{
					_valid = Messages.Count == 0;
					_dirty = false;
				}
				return _valid;
			}
		}

		/// <summary>
		/// Gets a list of validation messages, which contain the property that was invalid and the reason why it was invalid.
		/// </summary>
		public IList<ValidationMessage> Messages { get; internal set; }

		/// <summary>
		/// Adds an error to a result.
		/// </summary>
		/// <param name="errorMessage">The error message string.</param>
		public void AddError(string errorMessage)
		{
			AddError(errorMessage, "");
		}

		/// <summary>
		/// Adds an error to a result.
		/// </summary>
		/// <param name="errorMessage">The error message string.</param>
		/// <param name="propertyName">The name of the property.</param>
		public void AddError(string errorMessage, string propertyName)
		{
			_dirty = true;
			Messages.Add(new ValidationMessage { Message = errorMessage, PropertyName = propertyName });
		}

		/// <summary>
		/// Returns all <see cref="Messages"/> as a single string
		/// </summary>
		/// <returns>all validation messages concatenated as a single string</returns>
		public string ConcatMessages()
		{
			var msg = string.Empty;

			foreach (var m in Messages)
			{
				if (msg.Length != 0)
				{
					msg += " ";
				}
				msg += m.Message;
			}

			return msg;
		}
	}
}