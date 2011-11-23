using System;
using JobSystem.Framework.Security;

namespace JobSystem.Framework
{
	/// <summary>
	/// Handles cross cutting security concerns.
	/// </summary>
	public static class AppSecurity
	{
		/// <summary>
		/// Returns the default membership provider for the application.
		/// </summary>
		public static Func<IMembershipProvider> GetMembershipProvider = () => DefaultMembershipProvider.Provider;
		/// <summary>
		/// Returns the default password generator for the application.
		/// </summary>
		public static Func<IPasswordGenerator> GetPasswordGenerator = () => new PasswordGenerator();
	}
}