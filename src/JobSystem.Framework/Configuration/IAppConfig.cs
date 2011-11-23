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
namespace JobSystem.Framework.Configuration
{
	/// <summary>
	/// A strongly typed interface for retrieving application configuration information.
	/// </summary>
	public interface IAppConfig
	{
		#region AppSettings

		/// <summary>
		/// Gets the Amazon public key
		/// </summary>
		string AmazonKey { get; }

		/// <summary>
		/// Gets the Amazon private key
		/// </summary>
		string AmazonPrivateKey { get; }

		/// <summary>
		/// Gets the database catalog name
		/// </summary>
		string DatabaseCatalogName { get; }

		/// <summary>
		/// Gets the database password for <see cref="DatabaseUsername"/>
		/// </summary>
		string DatabasePassword { get; }

		/// <summary>
		/// Gets the database server name
		/// </summary>
		string DatabaseServer { get; }

		/// <summary>
		/// Gets the database username
		/// </summary>
		string DatabaseUsername { get; }

		#endregion
	}
}