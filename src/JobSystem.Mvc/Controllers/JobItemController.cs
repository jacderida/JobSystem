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
using JobSystem.Mvc.ViewModels.Certificates;
using JobSystem.Mvc.ViewModels.Consignments;
using JobSystem.Mvc.ViewModels.Deliveries;
using JobSystem.Mvc.ViewModels.JobItems;
using JobSystem.Mvc.ViewModels.Orders;
using JobSystem.Mvc.ViewModels.Quotes;
using JobSystem.Mvc.ViewModels.WorkItems;

namespace JobSystem.Mvc.Controllers
{
    public class JobItemController : Controller
    {
        private readonly UserManagementService _userManagementService;
        private readonly JobItemService _jobItemService;
        private readonly ListItemService _listItemService;
        private readonly InstrumentService _instrumentService;
        private readonly ConsignmentService _consignmentService;
        private readonly QuoteItemService _quoteItemService;
        private readonly OrderItemService _orderItemService;
        private readonly DeliveryItemService _deliveryItemService;
        private readonly CertificateService _certificateService;

        public JobItemController(
            UserManagementService userManagementService,
            JobItemService jobItemService,
            ListItemService listItemService,
            InstrumentService instrumentService,
            ConsignmentService consignmentService,
            QuoteItemService quoteItemService,
            OrderItemService orderItemService,
            DeliveryItemService deliveryItemService,
            CertificateService certificateService)
        {
            _jobItemService = jobItemService;
            _listItemService = listItemService;
            _instrumentService = instrumentService;
            _consignmentService = consignmentService;
            _quoteItemService = quoteItemService;
            _orderItemService = orderItemService;
            _deliveryItemService = deliveryItemService;
            _certificateService =  certificateService;
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchByKeyword(string keyword)
        {
            var results = _jobItemService.SearchByKeyword(keyword).Select(
                ji => new JobItemSearchResultsViewModel
                {
                    ItemNo = ji.ItemNo,
                    JobItemRef = ji.GetJobItemRef(),
                    Instrument = ji.Instrument.ToString(),
                    JobNumber = ji.Job.JobNo,
                    JobId = ji.Job.Id,
                    SerialNo = ji.SerialNo
                }).OrderBy(ji => ji.JobItemRef);
            return PartialView("_SearchResults", results);
        }

        [HttpGet]
        public ActionResult Create(Guid id)
        {
            var viewmodel = new JobItemViewModel()
            {
                Fields = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemCategory).OrderBy(li => li.Name).ToSelectList(),
                Instruments = _instrumentService.GetInstruments().ToSelectList(),
                Status = _listItemService.GetAllByCategory(ListItemCategoryType.JobItemInitialStatus).OrderBy(li => li.Name).ToSelectList(),
                JobId = id
            };
            return View(viewmodel);
        }

        [HttpPost]
        [Transaction]
        public ActionResult Create(JobItemViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var id = Guid.NewGuid();
                    _jobItemService.CreateJobItem(
                        viewmodel.JobId,
                        id,
                        viewmodel.InstrumentId,
                        viewmodel.SerialNo,
                        viewmodel.AssetNo,
                        viewmodel.InitialStatusId,
                        viewmodel.FieldId,
                        viewmodel.CalPeriod,
                        viewmodel.Instructions,
                        viewmodel.Accessories,
                        viewmodel.IsReturned,
                        viewmodel.ReturnReason,
                        viewmodel.Comments);
                    return RedirectToAction("Details", "Job", new { id = viewmodel.JobId });
                }
                catch (DomainValidationException dex)
                {
                    ModelState.UpdateFromDomain(dex.Result);
                }
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult EditInformation(Guid id)
        {
            var jobItem = _jobItemService.GetById(id);
            var viewmodel = new JobItemViewModel()
            {
                Id = id,
                Accessories = jobItem.Accessories,
                Comments = jobItem.Comments,
                Instructions = jobItem.Instructions,
                IsReturned = jobItem.IsReturned,
                ReturnReason = jobItem.ReturnReason,
                JobId = jobItem.Job.Id
            };
            return PartialView("_EditInformation", viewmodel);
        }

        [HttpPost]
        [Transaction]
        public ActionResult EditInformation(JobItemDetailsViewModel viewmodel)
        {
            _jobItemService.EditInformation(viewmodel.Id, viewmodel.Instructions, viewmodel.Accessories, viewmodel.Comments);
            return PartialView("_InformationTab", viewmodel);
        }

		[HttpGet]
		public ActionResult EditInstrumentPartial(Guid id)
		{
			var jobItem = _jobItemService.GetById(id);
			var viewmodel = new EditInstrumentViewModel()
			{
				JobItemId = id,
				JobId = jobItem.Job.Id
			};
			return PartialView("_EditInstrument", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult EditInstrument(EditInstrumentViewModel viewmodel)
		{
			_jobItemService.EditInstrument(viewmodel.JobItemId, viewmodel.InstrumentId);
			var instrumentDetails = _jobItemService.GetById(viewmodel.JobItemId).Instrument.ToString();
			return Json(instrumentDetails);
		}

        [HttpGet]
        public ActionResult Details(Guid Id)
        {
            var jobItem = _jobItemService.GetById(Id);
            var viewModel = new JobItemDetailsViewModel
            {
                Id = jobItem.Id,
                JobId = jobItem.Job.Id,
                UserRole = GetLoggedInUserRoles(),
                Accessories = jobItem.Accessories,
                AssetNo = jobItem.AssetNo,
                CalPeriod = jobItem.CalPeriod,
                Field = jobItem.Field.Name,
                Status = jobItem.Status.Name,
                SerialNo = jobItem.SerialNo,
                Comments = jobItem.Comments,
                Instructions = jobItem.Instructions,
                IsReturned = jobItem.IsReturned,
                ReturnReason = jobItem.ReturnReason,
                IsInvoiced = jobItem.IsInvoiced,
                IsMarkedForInvoicing = jobItem.IsMarkedForInvoicing,
                QuoteItem = PopulateQuoteItemViewModel(jobItem.Id),
                Delivery = PopulateDeliveryItemViewModel(jobItem.Id),
                Certificates = PopulateCertificateViewModel(jobItem.Id),
                InstrumentDetails = jobItem.Instrument.ToString(),
                WorkItems = jobItem.HistoryItems.Select(wi => new WorkItemDetailsViewModel
                {
                    Id = wi.Id,
                    JobItemId = wi.JobItem.Id,
                    OverTime = wi.OverTime,
                    Report = wi.Report,
                    Status = wi.Status.Name.ToString(),
                    WorkTime = wi.WorkTime,
                    WorkType = wi.WorkType.Name.ToString(),
                    WorkBy = wi.User.Name,
                    DateCreated = wi.DateCreated.ToLongDateString() + ' ' + wi.DateCreated.ToShortTimeString()
                }).OrderByDescending(wi => wi.DateCreated).ToList()
            };
            PopulateOrderItemViewModel(viewModel);

            var pendingItem = _jobItemService.GetPendingConsignmentItem(Id);
            if (pendingItem == null)
            {
                var item = _jobItemService.GetLatestConsignmentItem(Id);
                if (item != null) 
                {
                    if (item.Consignment != null)
                    {
                        viewModel.Consignment = new ConsignmentIndexViewModel
                        {
                            Id = item.Consignment.Id,
                            ConsignmentNo = item.Consignment.ConsignmentNo,
                            CreatedBy = item.Consignment.CreatedBy.Name,
                            DateCreated = item.Consignment.DateCreated.ToLongDateString() + ' ' + item.Consignment.DateCreated.ToShortTimeString(),
                            SupplierName = item.Consignment.Supplier.Name,
                            IsOrdered = item.Consignment.IsOrdered
                        };
                    }
                    else
                    {
                        viewModel.ConsignmentItem = new ConsignmentItemIndexViewModel
                        {
                            Id = item.Id,
                            Instructions = item.Instructions,
                            SupplierName = item.Consignment.Supplier.Name,
                            IsOrdered = item.Consignment.IsOrdered
                        };
                    }
                } 
                else 
                {
                    viewModel.ConsignmentItem = null;
                }
            }
            else
            {
                viewModel.ConsignmentItem = new ConsignmentItemIndexViewModel()
                {
                    Id = pendingItem.Id,
                    Instructions = pendingItem.Instructions,
                    SupplierName = pendingItem.Supplier.Name
                };
            }
            return PartialView("_Details", viewModel);
        }

        private UserRole GetLoggedInUserRoles()
        {
            var emailAddress = HttpContext.User.Identity.Name;
            var user = _userManagementService.GetByEmail(emailAddress);
            return user.Roles;
        }

        private QuoteItemIndexViewModel PopulateQuoteItemViewModel(Guid id)
        {
            var pendingItem = _quoteItemService.GetPendingQuoteItemForJobItem(id);
            if (pendingItem == null)
            {
                var item = _quoteItemService.GetQuoteItemsForJobItem(id).OrderByDescending(qi => qi.Quote.DateCreated).FirstOrDefault();
                if (item != null)
                {
                    return new QuoteItemIndexViewModel
                    {
                        Id = item.Id,
                        JobItemId = item.JobItem.Id,
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
                        JobItemRef = String.Format("{0}/{1}", item.JobItem.Job.JobNo, item.JobItem.ItemNo),
                        Status = item.Status.Name,
                        StatusType = item.Status.Type,
                        QuoteNo = item.Quote.QuoteNumber
                    };
                }
                else
                    return null;
            }
            else
            {
                return new QuoteItemIndexViewModel
                {
                    Id = pendingItem.Id,
                    JobItemId = pendingItem.JobItem.Id,
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
                    JobItemRef = String.Format("{0}/{1}", pendingItem.JobItem.Job.JobNo, pendingItem.JobItem.ItemNo),
                };
            }
        }

        private void PopulateOrderItemViewModel(JobItemDetailsViewModel jobItemViewModel)
        {
            var pendingItem = _orderItemService.GetPendingOrderItemForJobItem(jobItemViewModel.Id);
            if (pendingItem == null)
            {
                var item = _orderItemService.GetOrderItemsForJobItem(jobItemViewModel.Id).OrderByDescending(oi => oi.Order.DateCreated).FirstOrDefault();
                if (item != null)
                {
                    jobItemViewModel.Order = new OrderIndexViewModel
                    {
                        Instructions = item.Order.Instructions,
                        OrderNo = item.Order.OrderNo,
                        SupplierName = item.Order.Supplier.Name,
                        CreatedBy = item.Order.CreatedBy.Name,
                        DateCreated = item.Order.DateCreated.ToLongDateString() + ' ' + item.Order.DateCreated.ToShortTimeString(),
                        Currency = item.Order.Currency.Name,
                        IsApproved = item.Order.IsApproved
                    };
                    jobItemViewModel.OrderItem = new OrderItemIndexViewModel
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
                }
            }
            else
            {
                jobItemViewModel.OrderItem = new OrderItemIndexViewModel
                {
                    Id = pendingItem.Id,
                    DeliveryDays = pendingItem.DeliveryDays.ToString(),
                    Description = pendingItem.Description,
                    Instructions = pendingItem.Instructions,
                    PartNo = pendingItem.PartNo,
                    Price = pendingItem.Price.ToString(),
                    Quantity = pendingItem.Quantity.ToString(),
                    JobItemId = pendingItem.JobItem.Id,
                    SupplierName = pendingItem.Supplier.Name,
                    IsPending = true
                };
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
            var items = _certificateService.GetCertificatesForJobItem(id).Select(i => new CertificateIndexViewModel(){
                CertificateNo = i.CertificateNumber,
                CreatedBy = i.CreatedBy.Name,
                DateCreated = i.DateCreated.ToLongDateString() + ' ' + i.DateCreated.ToShortTimeString(),
                TypeName = i.Type.Name,
                // IJ to remove
                //TestStandards = i.TestStandards.Select(ts => new TestStandardViewModel()
                //{
                //    CertificateNo = ts.CertificateNo,
                //    Description = ts.Description,
                //    SerialNo = ts.SerialNo
                //}).ToList()
            }).ToList();

            return items;
        }
    }
}
