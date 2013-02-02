using System.Collections.Generic;

namespace JobSystem.Mvc.ViewModels.Jobs
{
    public class JobListViewModel : PageViewModel
    {
        public IEnumerable<JobIndexViewModel> Jobs { get; set; }
        public JobCreateViewModel CreateViewModel { get; set; }
    }
}
