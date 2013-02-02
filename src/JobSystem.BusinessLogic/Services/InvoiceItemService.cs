using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Invoices;

namespace JobSystem.BusinessLogic.Services
{
    public class InvoiceItemService : ServiceBase
    {
        private readonly ICompanyDetailsRepository _companyDetailsRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IJobItemRepository _jobItemRepository;
        private readonly IListItemRepository _listItemRepository;
        private readonly IQuoteItemRepository _quoteItemRepository;

        public InvoiceItemService(
            IUserContext userContext,
            ICompanyDetailsRepository companyDetailsRepository,
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository,
            IJobItemRepository jobItemRepository,
            IQuoteItemRepository quoteItemRepository,
            IListItemRepository listItemRepository,
            IQueueDispatcher<IMessage> dispatcher) : base(userContext, dispatcher)
        {
            _companyDetailsRepository = companyDetailsRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _jobItemRepository = jobItemRepository;
            _listItemRepository = listItemRepository;
            _quoteItemRepository = quoteItemRepository;
        }

        public PendingInvoiceItem CreatePending(Guid id, Guid jobItemId)
        {
            if (_invoiceItemRepository.JobItemHasPendingInvoiceItem(jobItemId))
                throw new DomainValidationException(Messages.JobItemHasPendingItem, "JobItemId");
            if (id == Guid.Empty)
                throw new ArgumentException("A value must be supplied for the ID");
            if (!CurrentUser.HasRole(UserRole.Manager))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            var jobItem = GetJobItem(jobItemId);
            var quoteItem = GetQuoteItem(jobItemId);
            var pendingItem = new PendingInvoiceItem();
            pendingItem.Id = id;
            pendingItem.Description = jobItem.Instrument.ToString();
            pendingItem.JobItem = jobItem;
            pendingItem.OrderNo = GetOrderNo(quoteItem);
            AssignPrices(pendingItem, quoteItem);
            _invoiceItemRepository.CreatePendingItem(pendingItem);
            jobItem.IsMarkedForInvoicing = true;
            _jobItemRepository.Update(jobItem);
            return pendingItem;
        }

        public InvoiceItem CreateFromPending(
            Guid id, Guid invoiceId, string description, decimal calibrationPrice, decimal repairPrice, decimal partsPrice, decimal carriagePrice, decimal investigationPrice, JobItem jobItem)
        {
            var invoice = GetInvoice(invoiceId);
            var invoiceItem = new InvoiceItem();
            invoiceItem.Id = id;
            invoiceItem.ItemNo = _invoiceRepository.GetInvoiceItemCount(invoiceId) + 1;
            invoiceItem.Invoice = invoice;
            invoiceItem.Description = description;
            invoiceItem.CalibrationPrice = calibrationPrice;
            invoiceItem.RepairPrice = repairPrice;
            invoiceItem.PartsPrice = partsPrice;
            invoiceItem.CarriagePrice = carriagePrice;
            invoiceItem.InvestigationPrice = investigationPrice;
            invoiceItem.JobItem = jobItem;
            jobItem.Status = _listItemRepository.GetByType(ListItemType.StatusInvoiced);
            jobItem.IsInvoiced = true;
            _jobItemRepository.EmitItemHistory(
                CurrentUser, jobItem.Id, 0, 0, String.Format("Item invoiced on {0}", invoice.InvoiceNumber), ListItemType.StatusInvoiced, ListItemType.WorkTypeAdministration);
            _jobItemRepository.Update(jobItem);
            _invoiceItemRepository.Create(invoiceItem);
            return invoiceItem;
        }

        public int GetInvoiceItemsCount(Guid invoiceId)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _invoiceItemRepository.GetInvoiceItemsCount(invoiceId);
        }

        public IEnumerable<InvoiceItem> GetInvoiceItems(Guid invoiceId)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _invoiceItemRepository.GetInvoiceItems(invoiceId);
        }

        public int GetPendingInvoiceItemsCount()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _invoiceItemRepository.GetPendingInvoiceItemsCount();
        }

        public IEnumerable<PendingInvoiceItem> GetPendingInvoiceItems()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _invoiceItemRepository.GetPendingInvoiceItems();
        }

        public IEnumerable<PendingInvoiceItem> GetPendingInvoiceItems(IList<Guid> pendingItemIds)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _invoiceItemRepository.GetPendingInvoiceItems(pendingItemIds);
        }

        public void DeletePendingItem(Guid id)
        {
            _invoiceItemRepository.DeletePendingItem(id);
        }

        private Invoice GetInvoice(Guid invoiceId)
        {
            var invoice = _invoiceRepository.GetById(invoiceId);
            if (invoice == null)
                throw new ArgumentException("A valid invoice ID must be supplied");
            return invoice;
        }

        private void AssignPrices(PendingInvoiceItem pendingItem, QuoteItem quoteItem)
        {
            if (_companyDetailsRepository.ApplyAllPrices())
            {
                pendingItem.CalibrationPrice = quoteItem.Calibration;
                pendingItem.RepairPrice = quoteItem.Labour;
                pendingItem.PartsPrice = quoteItem.Parts;
                pendingItem.CarriagePrice = quoteItem.Carriage;
                pendingItem.InvestigationPrice = quoteItem.Investigation;
                return;
            }
            if (quoteItem.Status.Type == ListItemType.StatusQuoteAccepted)
            {
                pendingItem.CalibrationPrice = quoteItem.Calibration;
                pendingItem.RepairPrice = quoteItem.Labour;
                pendingItem.PartsPrice = quoteItem.Parts;
                pendingItem.CarriagePrice = quoteItem.Carriage;
                pendingItem.InvestigationPrice = 0;
            }
            else
            {
                pendingItem.CalibrationPrice = 0;
                pendingItem.RepairPrice = 0;
                pendingItem.PartsPrice = 0;
                pendingItem.CarriagePrice = 0;
                pendingItem.InvestigationPrice = quoteItem.Investigation;
            }
        }

        private string GetOrderNo(QuoteItem quoteItem)
        {
            return !String.IsNullOrEmpty(quoteItem.Quote.OrderNumber) ? quoteItem.Quote.OrderNumber : quoteItem.JobItem.Job.OrderNo;
        }

        private JobItem GetJobItem(Guid jobItemId)
        {
            var jobItem = _jobItemRepository.GetById(jobItemId);
            if (jobItem == null)
                throw new ArgumentException("A valid ID must be supplied for the job item");
            if (jobItem.Job.IsPending)
                throw new DomainValidationException(Messages.JobPending, "JobItemId");
            return jobItem;
        }

        private QuoteItem GetQuoteItem(Guid jobItemId)
        {
            var quoteItem = _quoteItemRepository.GetQuoteItemsForJobItem(jobItemId).OrderByDescending(qi => qi.Quote.DateCreated).FirstOrDefault();
            if (quoteItem == null)
                throw new DomainValidationException(Messages.QuoteItemNull, "JobItemId");
            if (quoteItem.Status.Type == ListItemType.StatusQuotedPrepared)
                throw new DomainValidationException(Messages.QuoteStatusInvalid, "JobItemId");
            return quoteItem;
        }
    }
}