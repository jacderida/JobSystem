using System;
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
		private readonly IInvoiceItemRepository _invoiceItemRepository;
		private readonly IJobItemRepository _jobItemRepository;
		private readonly IQuoteItemRepository _quoteItemRepository;

		public InvoiceItemService(
			IUserContext userContext,
			ICompanyDetailsRepository companyDetailsRepository,
			IInvoiceItemRepository invoiceItemRepository,
			IJobItemRepository jobItemRepository,
			IQuoteItemRepository quoteItemRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(userContext, dispatcher)
		{
			_companyDetailsRepository = companyDetailsRepository;
			_invoiceItemRepository = invoiceItemRepository;
			_jobItemRepository = jobItemRepository;
			_quoteItemRepository = quoteItemRepository;
		}

		public PendingInvoiceItem CreatePending(Guid id, Guid jobItemId)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("A value must be supplied for the ID");
			ThrowIfJobItemInvalid(jobItemId);
			var quoteItem = GetQuoteItem(jobItemId);
			var jobItem = quoteItem.JobItem;
			var pendingItem = new PendingInvoiceItem();
			pendingItem.Id = id;
			pendingItem.Description = jobItem.Instrument.ToString();
			pendingItem.JobItem = jobItem;
			pendingItem.OrderNo = GetOrderNo(quoteItem);
			AssignPrices(pendingItem, quoteItem);
			_invoiceItemRepository.CreatePendingItem(pendingItem);
			return pendingItem;
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

		private void ThrowIfJobItemInvalid(Guid jobItemId)
		{
			var jobItem = _jobItemRepository.GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("A valid ID must be supplied for the job item");
		}

		private QuoteItem GetQuoteItem(Guid jobItemId)
		{
			var quoteItem = _quoteItemRepository.GetQuoteItemForJobItem(jobItemId);
			if (quoteItem == null)
				throw new DomainValidationException(Messages.QuoteItemNull, "JobItemId");
			if (quoteItem.Status.Type == ListItemType.StatusQuotedPrepared)
				throw new DomainValidationException(Messages.QuoteStatusInvalid, "JobItemId");
			return quoteItem;
		}
	}
}