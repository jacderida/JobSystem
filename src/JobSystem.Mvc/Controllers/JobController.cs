using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Jobs;

namespace JobSystem.Mvc.Controllers
{
	public class JobController : Controller
	{
		private readonly JobService _jobService;
		private readonly ListItemService _listItemService;
		private readonly CustomerService _customerServive;

		public JobController(JobService jobService, ListItemService listItemService, CustomerService customerService)
		{
			_jobService = jobService;
			_listItemService = listItemService;
			_customerServive = customerService;
		}

		[HttpGet]
		public ActionResult Create()
		{
			var jobViewModel = new JobCreateViewModel()
			{
				CreatedBy = "Graham Robertson",	// Hard coded for now.
				JobTypes = _listItemService.GetAllByType(ListItemType.JobType).ToSelectList(),
			};
			return View(jobViewModel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(JobCreateViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
					//viewModel.CustomerId = Guid.Parse("036B42CD-030B-4875-9759-3D9E790889A6");
					//viewModel.TypeId = Guid.Parse("3A8A156C-035F-4253-917A-96C2A04C2940");
					_jobService.CreateJob(id, viewModel.Instructions, viewModel.OrderNumber, viewModel.AdviceNumber, viewModel.TypeId, viewModel.CustomerId, viewModel.JobNote, viewModel.Contact);
					return RedirectToAction("PendingJobs");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View(viewModel);
		}

		public ActionResult Index()
		{
			//Placeholder admin role check to see whether user should be shown pending or approved jobs by default
			var isAdmin = true;
			if (isAdmin)
				return RedirectToAction("PendingJobs");
			return RedirectToAction("ApprovedJobs");
		}

		public ActionResult PendingJobs()
		{
			var jobs = _jobService.GetPendingJobs().Select(
				j => new JobIndexViewModel
				{
					CreatedBy = j.CreatedBy.ToString(),
					DateCreated = j.DateCreated.ToString(),
					JobNumber = j.JobNo,
					OrderNumber = j.OrderNo,
					Id = j.Id.ToString()
				}).ToList();

			var jobList = new JobListViewModel()
			{
				CreateViewModel = new JobCreateViewModel(),
				Jobs = jobs
			};
			jobList.CreateViewModel.JobTypes = _listItemService.GetAllByType(ListItemType.JobType).ToSelectList();
			jobList.CreateViewModel.Customers = _customerServive.GetCustomers().ToSelectList();

			return View(jobList);
		}

		public ActionResult ApprovedJobs()
		{
			var jobs = _jobService.GetApprovedJobs().Select(
				j => new JobIndexViewModel
				{
					CreatedBy = j.CreatedBy.ToString(),
					DateCreated = j.DateCreated.ToString(),
					JobNumber = j.JobNo,
					OrderNumber = j.OrderNo,
					Id = j.Id.ToString()
				}).ToList();

			var jobList = new JobListViewModel()
			{
				CreateViewModel = new JobCreateViewModel(),
				Jobs = jobs
			};
			jobList.CreateViewModel.JobTypes = _listItemService.GetAllByType(ListItemType.JobType).ToSelectList();
			jobList.CreateViewModel.Customers = _customerServive.GetCustomers().ToSelectList();

			return View(jobList);
		}

		public ActionResult Details()
		{
			return View();
		}
	}
}