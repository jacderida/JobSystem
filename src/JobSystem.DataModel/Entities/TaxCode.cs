﻿using System;

namespace JobSystem.DataModel.Entities
{
	public class TaxCode
	{
		public virtual Guid Id { get; set; }
		public virtual string TaxCode { get; set; }
		public virtual string Description { get; set; }
		public virtual double Rate { get; set; }
	}
}