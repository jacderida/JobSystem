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
using System.ComponentModel;
using System.Configuration;

namespace JobSystem.Framework.Configuration
{
	/// <summary>
	/// <see cref="IAppConfig"/> implementation that reads configuration values from a local configuration file.
	/// This class therefore provides functionality similar to the framework <see cref="ConfigurationManager.AppSettings"/>,
	/// but conforming to the <see cref="IAppConfig"/> interface.
	/// </summary>
	public class LocalAppConfig : IAppConfig, IDisposable
	{
		#region AppSettings

		/// <summary>
		/// Gets the Amazon public key
		/// </summary>
		public string AmazonKey
		{
			get { return GetSetting("AWSK", "", true); }
		}

		/// <summary>
		/// Gets the Amazon private key
		/// </summary>
		public string AmazonPrivateKey
		{
			get { return GetSetting("AWSPK", "", true); }
		}

		/// <summary>
		/// Gets name of the bucket that contains the serialised view models.
		/// </summary>
		/// <value>The view model bucket.</value>
		public string ViewModelBucket
		{
			get { return GetSetting("ViewModelBucket", "", true); }
		}

		/// <summary>
		/// Gets the Risk database catalog name
		/// </summary>
		public string DatabaseCatalogName
		{
			get { return GetSetting("DatabaseCatalogName", "", true); }
		}

		/// <summary>
		/// Gets the Risk database password for <see cref="DatabaseUsername"/>
		/// </summary>
		public string DatabasePassword
		{
			get { return GetSetting("DatabasePassword", "", true); }
		}

		/// <summary>
		/// Gets the Risk database server name
		/// </summary>
		public string DatabaseServer
		{
			get { return GetSetting("DatabaseServer", "", true); }
		}

		/// <summary>
		/// Gets the Risk database username
		/// </summary>
		public string DatabaseUsername
		{
			get { return GetSetting("DatabaseUsername", "", true); }
		}

		#endregion
		#region ConnectionStrings

		public string SqlEventStoreConnectionString
		{
			get { return GetConnectionString("SqlEventStore"); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Disposes this instance.
		/// </summary>
		public void Dispose()
		{
			// this implementation exists just as a tag for autofac.
		}

		/// <summary>
		/// Gets the setting for the given name.
		/// </summary>
		/// <typeparam name="T">the type of the value from the configuration file to parse.</typeparam>
		/// <param name="settingName">The setting name in the configuration file.</param>
		/// <param name="defaultIfNull">A default value to return if there is no configuraton entry.</param>
		/// <param name="throwConfigurationErrorsExceptionIfNotFound">if set to <c>true</c> throws an ArgumentException if the Configuration Setting is not found.</param>
		/// <returns>A setting for the given name.</returns>
		internal static T GetSetting<T>(string settingName, T defaultIfNull, bool throwConfigurationErrorsExceptionIfNotFound = false)
		{
			var setting = ConfigurationManager.AppSettings[settingName];
			if (setting == null)
			{
				if (!throwConfigurationErrorsExceptionIfNotFound)
					return defaultIfNull;
				throw new ConfigurationErrorsException(String.Format(
					"The key/value pair for '{0}' is missing from the appSettings section of the current app.config or web.config file.",
					settingName));
			}
			return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(setting);
		}

		internal string GetConnectionString(string name)
		{
			var connectionString = ConfigurationManager.ConnectionStrings[name];
			if (connectionString == null)
				throw new ConfigurationErrorsException(
					String.Format("The connection string for '{0}' is missing from the connectionStrings section of the current app.config or web.config file.", name));
			return connectionString.ConnectionString;
		}

		#endregion
	}
}