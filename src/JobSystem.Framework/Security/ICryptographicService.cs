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
namespace JobSystem.Framework.Security
{
	/// <summary>
	/// Service for generating password hashes. 
	/// </summary>
	public interface ICryptographicService
	{
		/// <summary>
		/// Generate a salt for a password. 
		/// </summary>
		/// <returns>A base 64 encoded salt</returns>
		string GenerateSalt();
		/// <summary>
		/// Compute a hash for the given value
		/// </summary>
		/// <param name="value">The value to hash</param>
		/// <returns>The hashed value</returns>
		string ComputeHash(string value);
		/// <summary>
		/// Compute a hash of the password and given salt
		/// </summary>
		/// <param name="password">The password</param>
		/// <param name="salt">The salt to append to the password</param>
		/// <returns>The computed hash</returns>
		string ComputeHash(string password, string salt);
	}
}
