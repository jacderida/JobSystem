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
	/// A object that can validate an object of a specific type.  
	/// </summary>
	/// <typeparam name="T">The type of the validator.</typeparam>
	public interface IValidator<T>
	{
		/// <summary>
		/// Validates an object of Type T.
		/// </summary>
		/// <param name="obj">The object to validate</param>
		/// <returns>A ValidationResult containing any validation errors.</returns>
		ValidationResult Validate(T obj);
	}
}
