using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Storage;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Consignments;
using JobSystem.Mvc.ViewModels.JobItems;
using JobSystem.Mvc.ViewModels.Jobs;
using JobSystem.Mvc.ViewModels.WorkItems;
using JobSystem.Mvc.ViewModels;
using JobSystem.Mvc.ViewModels.Quotes;

namespace JobSystem.Mvc.Controllers
{
	public class JobController : Controller
	{
		private readonly JobService _jobService;
		private readonly ListItemService _listItemService;
		private readonly CustomerService _customerServive;
		private readonly JobItemService _jobItemService;
		private readonly QuoteItemService _quoteItemService;

		public JobController(JobService jobService, ListItemService listItemService, CustomerService customerService, JobItemService jobItemService, QuoteItemService quoteItemService)
		{
			_jobService = jobService;
			_listItemService = listItemService;
			_customerServive = customerService;
			_jobItemService = jobItemService;
			_quoteItemService = quoteItemService;
		}

		[HttpGet]
		public ActionResult Create()
		{
			var jobViewModel = new JobCreateViewModel()
			{
				CreatedBy = "Graham Robertson",	// Hard coded for now.
				JobTypes = _listItemService.GetAllByCategory(ListItemCategoryType.JobType).ToSelectList(),
			};
			return View(jobViewModel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(JobCreateViewModel viewModel, Guid[] AttachmentId, string[] AttachmentName)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var id = Guid.NewGuid();
					_jobService.CreateJob(id, viewModel.Instructions, viewModel.OrderNumber, viewModel.AdviceNumber, viewModel.TypeId, viewModel.CustomerId, viewModel.JobNote, viewModel.Contact);
					
					if (AttachmentId != null)
					{
						for (var i = 0; i < AttachmentId.Length; i++)
						{
							_jobService.AddAttachment(id, AttachmentId[i], AttachmentName[i]);
						}
					}
					
					return RedirectToAction("PendingJobs");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return View(viewModel);
		}

		//Not yet in use
		public ActionResult Edit(Guid id)
		{
			var job = _jobService.GetJob(id);

			var viewmodel = new JobEditViewModel(){
				JobTypes = _listItemService.GetAllByCategory(ListItemCategoryType.JobType).ToSelectList(),
				//Customers = job.Customer,
				OrderNumber = job.OrderNo,
				AdviceNumber = job.AdviceNo,
				Contact = job.Contact,
				Instructions = job.Instructions,
				JobNote = job.Notes
				//Attachments = job.Attachments
			};

			return PartialView("_Edit", viewmodel);
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
			jobList.CreateViewModel.JobTypes = _listItemService.GetAllByCategory(ListItemCategoryType.JobType).ToSelectList();
			jobList.CreateViewModel.Customers = _customerServive.GetCustomers().ToSelectList();

			return View(jobList);
		}

		public ActionResult ApprovedJobs(int page = 1)
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
			jobList.CreateViewModel.JobTypes = _listItemService.GetAllByCategory(ListItemCategoryType.JobType).ToSelectList();
			jobList.CreateViewModel.Customers = _customerServive.GetCustomers().ToSelectList();

			return View(jobList);
		}

		public ActionResult Details(Guid id, string TabNo)
		{
			var job = _jobService.GetJob(id);
			var jobItems = _jobItemService.GetJobItems(id);
			var viewModel = new JobDetailsViewModel()
			{
				Id = job.Id.ToString(),
				CreatedBy = job.CreatedBy.ToString(),
				DateCreated = job.DateCreated.ToLongDateString() + ' ' + job.DateCreated.ToShortTimeString(),
				JobNumber = job.JobNo,
				OrderNumber = job.OrderNo,
				AdviceNumber = job.AdviceNo,
				Instruction = job.Instructions,
				Note = job.Notes,
				Contact = job.Contact,
				CustomerName = job.Customer.Name,
				CustomerAddress1 = job.Customer.Address1,
				CustomerAddress2 = job.Customer.Address2,
				CustomerAddress3 = job.Customer.Address3,
				CustomerAddress4 = job.Customer.Address4,
				CustomerAddress5 = job.Customer.Address5,
				CustomerEmail = job.Customer.Email,
				CustomerTelephone = job.Customer.Telephone,
				IsPending = job.IsPending,
				JobItems = jobItems.Select(ji => new JobItemDetailsViewModel
					{
						Id = ji.Id,
						JobId = id,
						AssetNo = ji.AssetNo,
						SerialNo = ji.SerialNo,
						InitialStatus = ji.Status.Name.ToString(),
						CalPeriod = ji.CalPeriod,
						Field = ji.Field.Name.ToString(),
						Location = ji.Location.Name.ToString(),
						InstrumentDetails = String.Format("{0} - {1} : {2}", ji.Instrument.ModelNo, ji.Instrument.Manufacturer.ToString(), ji.Instrument.Description),
						QuoteItem = PopulateQuoteItemViewModel(ji.Id),
						ConsignmentItem = PopulateConsignmentItemViewModel(ji.Id),
						WorkItems = ji.HistoryItems.Select(wi => new WorkItemDetailsViewModel
						{
							Id = wi.Id,
							JobItemId = wi.JobItem.Id,
							OverTime = wi.OverTime,
							Report = wi.Report,
							Status = wi.Status.Name.ToString(),
							WorkLocation = wi.WorkLocation.Name.ToString(),
							WorkTime = wi.WorkTime,
							WorkType = wi.WorkType.Name.ToString(),
							WorkBy = wi.User.Name.ToString(),
							DateCreated = wi.DateCreated.ToLongDateString() + ' ' + wi.DateCreated.ToShortTimeString()
							}).OrderByDescending(wi => wi.DateCreated).ToList()
					}).ToList(),
				Attachments = job.Attachments.Select(a => new AttachmentViewModel()
				{
					Id = a.Id,
					Name = a.Filename
				}).ToList()
			};
			return View(viewModel);
		}

		public ActionResult GetAttachment(Guid id, Guid attachmentId)
		{
			var attachment = _jobService.GetAttachment(id, attachmentId);
			var result = new FileStreamResult(attachment.Content, attachment.ContentType)
			{
				FileDownloadName = attachment.Filename
			};
			return result;
		}

		[Transaction]
		public ActionResult ApproveJob(Guid id)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_jobService.ApproveJob(id);
					return RedirectToAction("PendingJobs");
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return RedirectToAction("Details", new { id = id });
		}

		private ConsignmentItemIndexViewModel PopulateConsignmentItemViewModel(Guid Id)
		{
			var pendingItem = _jobItemService.GetPendingConsignmentItem(Id);
			if (pendingItem == null)
			{
				var item = _jobItemService.GetLatestConsignmentItem(Id);
				if (item != null) {
					var viewmodel = new ConsignmentItemIndexViewModel()
					{
						Id = item.Id,
						Instructions = item.Instructions,
						SupplierName = item.Consignment.Supplier.Name
					};
					return viewmodel;
				} else {
					return null;
				}
			}
			else
			{
				var viewmodel = new ConsignmentItemIndexViewModel()
				{
					Id = pendingItem.Id,
					Instructions = pendingItem.Instructions,
					SupplierName = pendingItem.Supplier.Name
				};
				return viewmodel;
			}
		}

		private QuoteItemIndexViewModel PopulateQuoteItemViewModel(Guid Id)
		{
			var pendingItem = _quoteItemService.GetPendingQuoteItemForJobItem(Id);
			if (pendingItem == null)
			{
				var item = _quoteItemService.GetQuoteItemForJobItem(Id);
				if (item != null)
				{
					var viewmodel = new QuoteItemIndexViewModel()
					{
						Id = item.Id,
						AdviceNo = item.Quote.AdviceNumber,
						Calibration = item.Calibration,
						Carriage = item.Carriage,
						Days = item.Days,
						Investigation = item.Investigation,
						ItemBER = item.BeyondEconomicRepair,
						OrderNo = item.Quote.OrderNumber,
						Parts = item.Parts,
						Repair = item.Labour,
						Report = item.Report,
						IsQuoted = true
					};
					return viewmodel;
				}
				else
				{
					return null;
				}
			}
			else
			{
				var viewmodel = new QuoteItemIndexViewModel()
				{
					Id = pendingItem.Id,
					AdviceNo = pendingItem.AdviceNo,
					Calibration = pendingItem.Calibration,
					Carriage = pendingItem.Carriage,
					Days = pendingItem.Days,
					Investigation = pendingItem.Investigation,
					ItemBER = pendingItem.BeyondEconomicRepair,
					OrderNo = pendingItem.OrderNo,
					Parts = pendingItem.Parts,
					Repair = pendingItem.Labour,
					Report = pendingItem.Report,
					IsQuoted = false
				};
				return viewmodel;
			}
		}
	}
}