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

namespace JobSystem.BusinessLogic.Validation.Core
{
	/// <summary>
	/// A message for a result of a validation, specifying the property and the reason why it was invalid.
	/// </summary>
	public sealed class ValidationMessage
	{
		/// <summary>
		/// Initialises an instance of the ValidationMessage class.
		/// </summary>
		internal ValidationMessage()
		{
		}

		/// <summary>
		/// Gets the reason why the property was invalid.
		/// </summary>
		public string Message { get; internal set; }

		/// <summary>
		/// Gets the name of the property that was invalid.
		/// </summary>
		public string PropertyName { get; internal set; }
	}
}
