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
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Helpers;
using JobSystem.DataModel.Entities;

namespace JobSystem.DataAccess.NHibernate.Mappings.Conventions
{
	/// <summary>
	/// Creates a FluentNHibernate AutoPersistenceModel that defines the conventions used.
	/// </summary>
	public class AutoPersistenceModelGenerator
	{
		/// <summary>
		/// Generate the FluentNHibernate AutoPersistenceModel
		/// </summary>
		/// <returns>The AutoPersistanceModel </returns>
		public AutoPersistenceModel Generate()
		{
			return AutoMap.AssemblyOf<UserAccount>(new SaturnAutomappingConfiguration())
				.Conventions
				.Setup(GetConventions())
				.UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
		}

		private static Action<FluentNHibernate.Conventions.IConventionFinder> GetConventions()
		{
			return mappings =>
			{
				mappings.Add<PrimaryKeyConvention>();
				mappings.Add<TableNameConvention>();
				mappings.Add<EnumConvention>();
				mappings.Add(ForeignKey.EndsWith("Id"));
				mappings.Add(DefaultCascade.None());
				mappings.Add(DefaultAccess.Property());
				mappings.Add(DefaultLazy.Always());
				mappings.Add(LazyLoad.Always());
			};
		}
	}
}