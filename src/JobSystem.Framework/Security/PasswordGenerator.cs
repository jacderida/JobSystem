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
using System.Linq;
using System.Web.Security;

namespace JobSystem.Framework.Security
{
	/// <summary>
	/// Password Generator that uses the ASP.net Membership class to generate a password
	/// <remarks>Be warned the password generate by Membership are pretty ugly</remarks>
	/// </summary>
	public class PasswordGenerator : IPasswordGenerator
	{
		private readonly int _minLength = 8;
		private readonly int _minNumbers = 1;

		/// <summary>
		/// constructor for this class
		/// </summary>
		/// <param name="minLength">The minimum length of password</param>
		/// <param name="minNums">The minuimum number the password has to contain</param>
		public PasswordGenerator(int minLength = 8, int minNums = 1)
		{
			_minLength = minLength;
			_minNumbers = minNums;
		}

		#region IPasswordGenerator Members

		/// <summary>
		/// Generate a random password
		/// </summary>
		/// <returns>The generated password</returns>
		public string Generate()
		{
			var password = Membership.GeneratePassword(_minLength, 0);
			var random = new Random();
			while (password.Count(c => Char.IsNumber(c)) < _minNumbers)
			{
				password += random.Next(0, 9);
			}
			return password;
		}

		#endregion
	}
}
