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
using JobSystem.Mvc.ViewModels.WorkItems;
using JobSystem.DataAccess.NHibernate;

namespace JobSystem.Mvc.Controllers
{
    public class JobController : Controller
    {
        private readonly JobService _jobService;
        private readonly ListItemService _listItemService;
        private readonly JobItemService _jobItemService;
        private readonly QuoteItemService _quoteItemService;
        private readonly OrderItemService _orderItemService;
        private readonly DeliveryItemService _deliveryItemService;
        private readonly CertificateService _certificateService;
        private readonly UserManagementService _userManagementService;

        public JobController(
            JobService jobService,
            ListItemService listItemService,
            JobItemService jobItemService,
            QuoteItemService quoteItemService,
            OrderItemService orderItemService,
            DeliveryItemService deliveryItemService,
            CertificateService certificateService,
            UserManagementService userManagementService)
        {
            _jobService = jobService;
            _listItemService = listItemService;
            _jobItemService = jobItemService;
            _quoteItemService = quoteItemService;
            _orderItemService = orderItemService;
            _deliveryItemService = deliveryItemService;
            _certificateService = certificateService;
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            var jobViewModel = new JobCreateViewModel()
            {
                CreatedBy = "Graham Robertson",    // Hard coded for now.
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
                    _jobService.CreateJob(id, viewModel.Instructions, viewModel.OrderNumber, viewModel.AdviceNumber, viewModel.TypeId, viewModel.CustomerId, viewModel.Notes, viewModel.Contact);
                    if (AttachmentId != null)
                        for (var i = 0; i < AttachmentId.Length; i++)
                            _jobService.AddAttachment(id, AttachmentId[i], AttachmentName[i]);
                    transaction.Commit();
                    return RedirectToAction("PendingJobs");
                }
                catch (DomainValidationException dex)
                {
                    transaction.Rollback();
                    ModelState.UpdateFromDomain(dex.Result);
                }
                finally
                {
                    transaction.Dispose();
                }
            }
            return View(viewModel);
        }

        public ActionResult Edit(Guid id)
        {
            var job = _jobService.GetJob(id);
            var viewModel = new JobEditViewModel
            { 
                Id = job.Id,
                OrderNumber = job.OrderNo,
                AdviceNumber = job.AdviceNo,
                Contact = job.Contact,
                Instructions = job.Instructions,
                Notes = job.Notes
            };
            return View(viewModel);
        }

        [HttpPost]
        [Transaction]
        public ActionResult Edit(JobEditViewModel model)
        {
            _jobService.Edit(model.Id, model.OrderNumber, model.AdviceNumber, model.Contact, model.Notes, model.Instructions);
            return RedirectToAction("Details", new { id = model.Id });
        }

        [Transaction]
        public ActionResult AttachAttachment(Guid jobId, Guid attachmentId, string attachmentName)
        {
            var job = _jobService.GetJob(jobId);
            _jobService.AddAttachment(jobId, attachmentId, attachmentName);
            return Json(true);
        }

        public ActionResult Index()
        {
            return RedirectToAction("ApprovedJobs");
        }

        public ActionResult PendingJobs(int page = 1)
        {
            var pageSize = 15;
            var jobs = _jobService.GetPendingJobs().Select(
                j => new JobIndexViewModel
                {
                    CreatedBy = j.CreatedBy.ToString(),
                    DateCreated = j.DateCreated.ToString(),
                    JobNumber = j.JobNo,
                    OrderNumber = j.OrderNo,
                    Id = j.Id.ToString()
                }).OrderBy(j => j.JobNumber).Skip((page - 1) * pageSize).Take(pageSize);
            var jobList = new JobListViewModel
            {
                CreateViewModel = new JobCreateViewModel(),
                Jobs = jobs,
                Page = page,
                PageSize = pageSize,
                Total = _jobService.GetApprovedJobsCount()
            };
            jobList.CreateViewModel.JobTypes = _listItemService.GetAllByCategory(ListItemCategoryType.JobType).ToSelectList();
            return View(jobList);
        }

        public ActionResult ApprovedJobs(int page = 1)
        {
            var pageSize = 15;
            var latestJobs = _jobService.GetApprovedJobs().Select(
                j => new JobIndexViewModel
                {
                    CreatedBy = j.CreatedBy.ToString(),
                    DateCreated = j.DateCreated.ToString(),
                    JobNumber = j.JobNo,
                    OrderNumber = j.OrderNo,
                    Id = j.Id.ToString()
                }).OrderByDescending(j => j.DateCreated).Take(pageSize);
            var jobList = new JobListViewModel
            {
                CreateViewModel = new JobCreateViewModel(),
                Jobs = latestJobs,
                Page = page,
                PageSize = pageSize,
                Total = _jobService.GetApprovedJobsCount()
            };
            jobList.CreateViewModel.JobTypes = _listItemService.GetAllByCategory(ListItemCategoryType.JobType).ToSelectList();
            return View(jobList);
        }

        public ActionResult Details(Guid id, string TabNo)
        {
            var job = _jobService.GetJob(id);
            var jobItems = _jobItemService.GetJobItems(id);
            var viewModel = new JobDetailsViewModel
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
                CustomerAssetLine = job.Customer.AssetLine,
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
                        JobItemRef = ji.GetJobItemRef(),
                        UserRole = GetLoggedInUserRoles(),
                        AssetNo = ji.AssetNo,
                        SerialNo = ji.SerialNo,
                        Status = ji.Status.Name.ToString(),
                        CalPeriod = ji.CalPeriod,
                        Field = ji.Field.Name.ToString(),
                        Accessories = ji.Accessories,
                        Comments = ji.Comments,
                        Instructions = ji.Instructions,
                        IsReturned = ji.IsReturned,
                        ReturnReason = ji.ReturnReason,
                        IsInvoiced = ji.IsInvoiced,
                        IsMarkedForInvoicing = ji.IsMarkedForInvoicing,
                        InstrumentDetails = ji.Instrument.ToString(),
                        QuoteItem = PopulateQuoteItemViewModel(ji.Id),
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
                    }).OrderBy(ji => ji.JobItemRef).ToList(),
                Attachments = job.Attachments.Select(
                    a => new AttachmentViewModel
                    {
                        Id = a.Id,
                        Name = a.Filename
                    }).ToList()
            };
            foreach (var ji in viewModel.JobItems)
            {
                PopulateOrderItemViewModel(ji);
                PopulateConsignmentItemViewModel(ji);
            }
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

        private UserRole GetLoggedInUserRoles()
        {
            var emailAddress = HttpContext.User.Identity.Name;
            var user = _userManagementService.GetByEmail(emailAddress);
            return user.Roles;
        }

        private void PopulateConsignmentItemViewModel(JobItemDetailsViewModel jiViewmodel)
        {
            var pendingItem = _jobItemService.GetPendingConsignmentItem(jiViewmodel.Id);
            if (pendingItem == null)
            {
                var item = _jobItemService.GetLatestConsignmentItem(jiViewmodel.Id);
                if (item != null)
                {
                    if (item.Consignment != null)
                    {
                        jiViewmodel.Consignment = new ConsignmentIndexViewModel()
                        {
                            Id = item.Consignment.Id,
                            ConsignmentNo = item.Consignment.ConsignmentNo,
                            CreatedBy = item.Consignment.CreatedBy.Name,
                            DateCreated = item.Consignment.DateCreated.ToLongDateString() + ' ' + item.Consignment.DateCreated.ToShortTimeString(),
                            SupplierName = item.Consignment.Supplier.Name,
                            IsOrdered = item.Consignment.IsOrdered
                        };
                        return;
                    }
                    jiViewmodel.ConsignmentItem = new ConsignmentItemIndexViewModel()
                    {
                        Id = item.Id,
                        Instructions = item.Instructions,
                        SupplierName = item.Consignment.Supplier.Name,
                        IsOrdered = item.Consignment.IsOrdered
                    };
                    return;
                }
                else
                    return;
            }
            else
            {
                jiViewmodel.ConsignmentItem = new ConsignmentItemIndexViewModel()
                {
                    Id = pendingItem.Id,
                    Instructions = pendingItem.Instructions,
                    SupplierName = pendingItem.Supplier.Name
                };
                return;
            }
        }

        private QuoteItemIndexViewModel PopulateQuoteItemViewModel(Guid id)
        {
            var pendingItem = _quoteItemService.GetPendingQuoteItemForJobItem(id);
            if (pendingItem == null)
            {
                var items = _quoteItemService.GetQuoteItemsForJobItem(id).OrderByDescending(qi => qi.Quote.DateCreated);
                if (items.Any())
                {
                    var item = items.First();
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
                        Carriage = item.Carriage.ToString(),
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
                    Carriage = pendingItem.Carriage.ToString(),
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
            var items = _certificateService.GetCertificatesForJobItem(id).Select(i => new CertificateIndexViewModel()
            {
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
                //}
                //).ToList()
            }).ToList();

            return items;
        }
    }
}