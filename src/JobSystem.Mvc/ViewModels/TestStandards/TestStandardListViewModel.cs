using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.TestStandards
{
	public class TestStandardListViewModel
	{
		public IEnumerable<TestStandardViewModel> Standards { get; set; }
		public TestStandardViewModel CreateViewModel { get; set; }
	}
}