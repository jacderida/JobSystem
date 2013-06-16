using System;
using System.Collections.Generic;
using JobSystem.Mvc.ViewModels.Instruments;

namespace JobSystem.Mvc.ViewModels.JobItems
{
	public class EditInstrumentViewModel
    {
        public Guid InstrumentId { get; set; }
        public Guid JobItemId { get; set; }
		public Guid JobId { get; set; }
    }
}
