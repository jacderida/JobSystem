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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace JobSystem.DataAccess.NHibernate
{
	/// <summary>
	/// The storage used to context static items
	/// </summary>
	/// <remarks>I have to use HttpContext.Current.Items in the context of a ASP.net request because it is possible for 
	/// a request to be handled by more than one threads. In these cases the HttpContext items are migrated but 
	/// CallContext items are not. See  
	/// http://piers7.blogspot.com/2005/11/threadstatic-callcontext-and_02.html</remarks>
	/// <typeparam name="T">The type of item stored</typeparam>
	public class LocalStorage<T> : ILocalStorage<T>
	{
		private const string CallContextKey = "Alea.DataAccess.NHibernate.StorageKey";

		private IDictionary CallContextDictionary
		{
			get { return CallContext.GetData(CallContextKey) as IDictionary; }
			set { CallContext.SetData(CallContextKey, value); }
		}

		private IDictionary Container
		{
			get
			{
				if (CallContextDictionary == null)
					CallContextDictionary = new Dictionary<string, object>();
				return CallContextDictionary;
			}
		}

		public T this[string key]
		{
			get
			{
				if (Container.Contains(key))
					return (T)Container[key];
				return default(T);
			}
			set
			{
				if (!Container.Contains(key))
					Container.Add(key, value);
				else
				{
					var dispose = Container[key] as IDisposable;
					if (dispose != null)
						dispose.Dispose();
					Container[key] = value;
				}
			}
		}
	}
}