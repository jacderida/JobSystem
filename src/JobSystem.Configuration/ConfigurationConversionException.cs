using System;

namespace JobSystem.Configuration
{
	[Serializable]
	public class ConfigurationConversionException : Exception
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationConversionException"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		public ConfigurationConversionException(Type type) : base(String.Format("Could not find a suitable conversion for the type {0}", type.Name))
		{
		}

		#endregion
	}
}