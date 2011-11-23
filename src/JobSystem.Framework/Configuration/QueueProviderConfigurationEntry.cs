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
	/// A config entry to specify the Queue provider
	/// </summary>
	public enum QueueProviderConfigurationEntry
	{
		/// <summary>
		/// Amazon Simple Queue Service (SQS)
		/// </summary>
		Sqs,
		/// <summary>
		/// Microsoft Message Queue Service (MSMQ)
		/// </summary>
		Msmq
	}
}