using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels;
using JobSystem.Mvc.ViewModels.Certificates;
using JobSystem.Mvc.ViewModels.Consignments;
using JobSystem.Mvc.ViewModels.Deliveries;
using JobSystem.Mvc.ViewModels.JobItems;
using JobSystem.Mvc.ViewModels.Jobs;
using JobSystem.Mvc.ViewModels.Orders;
using JobSystem.Mvc.ViewModels.Quotes;
using JobSystem.Mvc.ViewModels.TestStandards;
using JobSystem.Mvc.ViewModels.WorkItems;
using JobSystem.DataAccess.NHibernate;

namespace JobSystem.Mvc.Controllers
{
	public class JobController : Controller
	{
		private readonly JobService _jobService;
		private readonly ListItemService _listItemService;
		private readonly CustomerService _customerServive;
		private readonly JobItemService _jobItemService;
		private readonly QuoteItemService _quoteItemService;
		private readonly OrderItemService _orderItemService;
		private readonly DeliveryItemService _deliveryItemService;
		private readonly CertificateService _certificateService;

		public JobController(JobService jobService, ListItemService listItemService, CustomerService customerService, JobItemService jobItemService, QuoteItemService quoteItemService, OrderItemService orderItemService, DeliveryItemService deliveryItemService, CertificateService certificateService)
		{
			_jobService = jobService;
			_listItemService = listItemService;
			_customerServive = customerService;
			_jobItemService = jobItemService;
			_quoteItemService = quoteItemService;
			_orderItemService = orderItemService;
			_deliveryItemService = deliveryItemService;
			_certificateService = certificateService;
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
		public ActionResult Create(JobCreateViewModel viewModel, Guid[] AttachmentId, string[] AttachmentName)
		{
			if (ModelState.IsValid)
			{
				var transaction = NHibernateSession.Current.BeginTransaction();
				try
				{
					var id = Guid.NewGuid();
					_jobService.CreateJob(id, viewModel.Instructions, viewModel.OrderNumber, viewModel.AdviceNumber, viewModel.TypeId, viewModel.CustomerId, viewModel.JobNote, viewModel.Contact);
					if (AttachmentId != null)
						for (var i = 0; i < AttachmentId.Length; i++)
							_jobService.AddAttachment(id, AttachmentId[i], AttachmentName[i]);
					transaction.Commit();
					return RedirectToAction("PendingJobs");
				}
				catch (DomainValidationException dex)
				{
					transaction.Commit();
					ModelState.UpdateFromDomain(dex.Result);
				}
				finally
				{
					transaction.Dispose();
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
						InstrumentDetails = String.Format("{0} - {1} : {2}", ji.Instrument.ModelNo, ji.Instrument.Manufacturer.ToString(), ji.Instrument.Description),
						QuoteItem = PopulateQuoteItemViewModel(ji.Id),
						ConsignmentItem = PopulateConsignmentItemViewModel(ji.Id),
						OrderItem = PopulateOrderItemViewModel(ji.Id),
						Delivery = PopulateDeliveryItemViewModel(ji.Id),
						Certificates = PopulateCertificateViewModel(ji.Id),
						WorkItems = ji.HistoryItems.Select(wi => new WorkItemDetailsViewModel
						{
							Id = wi.Id,
							JobItemId = wi.JobItem.Id,
							OverTime = wi.OverTime,
							Report = wi.Report,
							Status = wi.Status.Name.ToString(),
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
						Calibration = (double)item.Calibration,
						Carriage = (double)item.Carriage,
						Days = item.Days,
						Investigation = (double)item.Investigation,
						ItemBER = item.BeyondEconomicRepair,
						OrderNo = item.Quote.OrderNumber,
						Parts = (double)item.Parts,
						Repair = (double)item.Labour,
						Report = item.Report,
						IsQuoted = true,
						JobItemNo = item.JobItem.ItemNo.ToString(),
						Status = item.Status.Name,
						QuoteNo = item.Quote.QuoteNumber
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
					Calibration = (double)pendingItem.Calibration,
					Carriage = (double)pendingItem.Carriage,
					Days = pendingItem.Days,
					Investigation = (double)pendingItem.Investigation,
					ItemBER = pendingItem.BeyondEconomicRepair,
					OrderNo = pendingItem.OrderNo,
					Parts = (double)pendingItem.Parts,
					Repair = (double)pendingItem.Labour,
					Report = pendingItem.Report,
					IsQuoted = false,
					JobItemNo = pendingItem.JobItem.ItemNo.ToString()
				};
				return viewmodel;
			}
		}

		private OrderItemIndexViewModel PopulateOrderItemViewModel(Guid Id)
		{
			var pendingItem = _orderItemService.GetPendingOrderItemForJobItem(Id);
			if (pendingItem == null)
			{
				var item = _orderItemService.GetOrderItemsForJobItem(Id).FirstOrDefault();
				if (item != null)
				{
					var viewmodel = new OrderItemIndexViewModel()
					{
						Id = item.Id,
						DeliveryDays = item.DeliveryDays.ToString(),
						Description = item.Description,
						Instructions = item.Instructions,
						PartNo = item.PartNo,
						Price = item.Price.ToString(),
						Quantity = item.Quantity.ToString(),
						JobItemId = item.JobItem.Id
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
				var viewmodel = new OrderItemIndexViewModel()
				{
					Id = pendingItem.Id,
					DeliveryDays = pendingItem.DeliveryDays.ToString(),
					Description = pendingItem.Description,
					Instructions = pendingItem.Instructions,
					PartNo = pendingItem.PartNo,
					Price = pendingItem.Price.ToString(),
					Quantity = pendingItem.Quantity.ToString(),
					JobItemId = pendingItem.JobItem.Id,
					SupplierName = pendingItem.Supplier.Name
				};
				return viewmodel;
			}
		}

		private DeliveryIndexViewModel PopulateDeliveryItemViewModel(Guid id)
		{
			var pendingItem = _deliveryItemService.GetPendingDeliveryItemForJobItem(id);
			if (pendingItem == null)
			{
				var item = _deliveryItemService.GetDeliveryItemForJobItem(id);
				if (item != null)
				{
					if (item.Delivery != null)
					{
						var viewmodel = new DeliveryIndexViewModel()
						{
							CustomerName = item.Delivery.Customer.Name,
							Fao = item.Delivery.Fao,
							Id = item.Id,
							Notes = item.Notes,
							CreatedBy = item.Delivery.CreatedBy.Name,
							DateCreated = item.Delivery.DateCreated.ToLongDateString() + ' ' + item.Delivery.DateCreated.ToShortTimeString(),
						};
						return viewmodel;
					}
					else
					{
						var viewmodel = new DeliveryIndexViewModel()
						{
							Id = item.Id,
							Notes = item.Notes
						};
						return viewmodel;
					}
				}
				else
				{
					return null;
				}
			}
			else
			{
				var viewmodel = new DeliveryIndexViewModel()
				{
					Id = pendingItem.Id,
					Notes = pendingItem.Notes
				};
				return viewmodel;
			}
		}

		private List<CertificateIndexViewModel> PopulateCertificateViewModel(Guid id)
		{
			var items = _certificateService.GetCertificatesForJobItem(id).Select(i => new CertificateIndexViewModel()
			{
				CertificateNo = i.CertificateNumber,
				CreatedBy = i.CreatedBy.Name,
				DateCreated = i.DateCreated.ToLongDateString() + ' ' + i.DateCreated.ToShortTimeString(),
				TypeName = i.Type.Name,
				TestStandards = i.TestStandards.Select(ts => new TestStandardViewModel()
				{
					CertificateNo = ts.CertificateNo,
					Description = ts.Description,
					SerialNo = ts.SerialNo
				}).ToList()
			}).ToList();

			return items;
		}
	}
}