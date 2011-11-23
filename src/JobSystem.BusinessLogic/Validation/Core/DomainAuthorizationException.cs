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

using System;

namespace JobSystem.BusinessLogic.Validation.Core
{
	/// <summary>
	/// The exception that is thrown when unauthorisation access to a domain object occurs.
	/// </summary>
	[Serializable]
	public class DomainAuthorizationException : DomainValidationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DomainAuthorizationException"/> class.
		/// </summary>
		public DomainAuthorizationException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainAuthorizationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public DomainAuthorizationException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainAuthorizationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="propertyName">Name of the property.</param>
		public DomainAuthorizationException(string message, string propertyName) : base(message, propertyName)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainAuthorizationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public DomainAuthorizationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainAuthorizationException"/> class.
		/// </summary>
		/// <param name="result">The result.</param>
		public DomainAuthorizationException(ValidationResult result) : base(result)
		{
		}
	}

}