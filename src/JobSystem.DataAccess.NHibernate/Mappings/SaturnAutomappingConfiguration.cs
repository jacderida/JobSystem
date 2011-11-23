using System;
using System.Linq;
using FluentNHibernate.Automapping;

namespace JobSystem.DataAccess.NHibernate.Mappings
{
	/// <summary>
	/// The Setup method in Fluent NHibernate's AutoPersisteanceModel is obselete. 
	/// This is now the preferred setup method (this week) for setting up the AutoMapping conventions.
	/// </summary>
	public class SaturnAutomappingConfiguration : DefaultAutomappingConfiguration
	{
		private static readonly string[] EntityNamespaces;

		static SaturnAutomappingConfiguration()
		{
			EntityNamespaces = new string[]
			{
				"JobSystem.DataModel.Entities",
			};
		}

		/// <summary>
		/// Check whether the given member is the id. 
		/// </summary>
		/// <param name="member">The member to check</param>
		/// <returns>True if the name is Id, false otherwise.</returns>
		public override bool IsId(FluentNHibernate.Member member)
		{
			return member.Name == "Id";
		}

		/// <summary>
		/// Method that ascertains whether the given type should be mapped. 
		/// </summary>
		/// <param name="type">The type to check</param>
		/// <returns>True if the given type matches or criteria.</returns>
		public override bool ShouldMap(Type type)
		{
			return EntityNamespaces.Contains(type.Namespace);
		}

		/// <summary>
		/// Gets the component column prefix.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <returns>returns an empty string</returns>
		public override string GetComponentColumnPrefix(FluentNHibernate.Member member)
		{
			return String.Empty;
		}
	}
}