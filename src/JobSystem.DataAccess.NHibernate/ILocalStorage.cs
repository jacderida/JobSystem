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

namespace JobSystem.DataAccess.NHibernate
{
	/// <summary>
	/// Interface for encapsulating the thread context storage
	/// </summary>
	/// <typeparam name="T">The type of object stored</typeparam>
	public interface ILocalStorage<T>
	{
		T this[string key]
		{
			get; 
			set;
		}
	}
}