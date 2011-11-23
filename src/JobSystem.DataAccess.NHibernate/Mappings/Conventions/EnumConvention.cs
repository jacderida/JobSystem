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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;

namespace JobSystem.DataAccess.NHibernate.Mappings.Conventions
{
	/// <summary>
	/// Convention that ensures that NHibernate saves enums as the base type in the database 
	/// </summary>
	public class EnumConvention : IUserTypeConvention
	{
		#region IConventionAcceptance<IPropertyInspector> Members

		/// <summary>
		/// The criteria used to accept a property
		/// </summary>
		/// <param name="criteria">The property to check</param>
		public void Accept(IAcceptanceCriteria<FluentNHibernate.Conventions.Inspections.IPropertyInspector> criteria)
		{
			criteria.Expect(x => x.Property.PropertyType.IsEnum);
		}

		#endregion

		#region IConvention<IPropertyInspector,IPropertyInstance> Members

		/// <summary>
		/// Apply the convention to an accepted property
		/// </summary>
		/// <param name="instance">the instance</param>
		public void Apply(IPropertyInstance instance)
		{
			instance.CustomType(instance.Property.PropertyType);
		}

		#endregion
	}
}