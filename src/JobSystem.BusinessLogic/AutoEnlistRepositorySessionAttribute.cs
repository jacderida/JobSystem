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
using JobSystem.BusinessLogic.Services;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace JobSystem.BusinessLogic
{
	/// <summary>
	/// Creates a new session if an exisiting one is not being reused.
	/// </summary>
	[Serializable]
	[ProvideAspectRole(StandardRoles.TransactionHandling)]
	[MulticastAttributeUsage(MulticastTargets.Method, Inheritance = MulticastInheritance.Multicast, TargetMemberAttributes = MulticastAttributes.Public)]
	[AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Security)]
	public class AutoEnlistRepositorySessionAttribute : OnMethodBoundaryAspect
	{
		/// <summary>
		/// Called when a business logic method is entered
		/// </summary>
		/// <param name="args">The details of the method used to obtain a reference to the callee object.</param>
		public override void OnEntry(MethodExecutionArgs args)
		{
			//******************************************************************************************************
			//TODO: The repository session scope should be set when the first method on the service is called. 
			//Thereafter you should not be allowed to change it.
			//(this aspect could tell us how many calls have been executed. i.e. 1 or more...first call?)
			//However the property you want to lock is on the service base class. (so would have to pass a flag back to the base class)

			var service = (ServiceBase)args.Instance;
			if (service.RepositorySessionScope == null)
			{
				bool sessionWasStartedByThisMethodCall = true;
				args.MethodExecutionTag = sessionWasStartedByThisMethodCall;
				service.RepositorySessionScope = service.RepositorySessionFactory.NewSessionScope();
				service.RepositorySessionScope.BeginTransaction();
			}
			base.OnEntry(args);
		}

		/// <summary>
		/// Called when a business logic method exits
		/// </summary>
		/// <param name="args">The details of the method used to obtain a reference to the callee object.</param>
		public override void OnExit(MethodExecutionArgs args)
		{
			var service = (ServiceBase)args.Instance;
			bool sessionStartedByMethodCall = WasSessionScopeStartedByMethodCall(args);
			if (sessionStartedByMethodCall)
			{
				service.RepositorySessionScope.Dispose();
				service.RepositorySessionScope = null;
				args.MethodExecutionTag = false;
			}
			base.OnExit(args);
		}

		/// <summary>
		/// Called when a business logic method sucessfully exits.
		/// </summary>
		/// <param name="args">The details of the method used to obtain a reference to the callee object.</param>
		public override void OnSuccess(MethodExecutionArgs args)
		{
			var service = (ServiceBase)args.Instance;
			bool sessionStartedByMethodCall = WasSessionScopeStartedByMethodCall(args);
			if (!sessionStartedByMethodCall)
				return;
			service.RepositorySessionScope.CommitTransaction();
			base.OnSuccess(args);
		}

		/// <summary>
		/// Called when a business logic method throws an unhandled exeption
		/// </summary>
		/// <param name="args">The details of the method used to obtain a reference to the callee object.</param>
		public override void OnException(MethodExecutionArgs args)
		{
			var service = (ServiceBase)args.Instance;
			bool sessionStartedByMethodCall = WasSessionScopeStartedByMethodCall(args);
			if (!sessionStartedByMethodCall)
				return;
			service.RepositorySessionScope.RollbackTransaction();
			base.OnException(args);
		}

		private static bool WasSessionScopeStartedByMethodCall(MethodExecutionArgs args)
		{
			bool sessionStartedByMethodCall = false;
			if (args.MethodExecutionTag != null)
				sessionStartedByMethodCall = (bool)args.MethodExecutionTag;
			return sessionStartedByMethodCall;
		}
	}
}