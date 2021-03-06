﻿using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.QuoteItems;

namespace JobSystem.BusinessLogic.Services
{
    public class QuoteItemService : ServiceBase
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteItemRepository _quoteItemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IJobItemRepository _jobItemRepository;
        private readonly IListItemRepository _listItemRepository;

        public QuoteItemService(
            IUserContext applicationContext,
            IQuoteRepository quoteRepository,
            IQuoteItemRepository quoteItemRepository,
            IJobItemRepository jobItemRepository,
            IListItemRepository listItemRepository,
            ICustomerRepository customerRepository,
            IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
        {
            _quoteRepository = quoteRepository;
            _quoteItemRepository = quoteItemRepository;
            _customerRepository = customerRepository;
            _jobItemRepository = jobItemRepository;
            _listItemRepository = listItemRepository;
        }

        public QuoteItem Create(Guid id, Guid quoteId, Guid jobItemId, decimal labour, decimal calibration, decimal parts, decimal carriage, decimal investigation, string report, int days, bool beyondEconomicRepair)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            if (id == Guid.Empty)
                throw new ArgumentException("An ID must be supplied for the quote item");
            var quote = GetQuote(quoteId);
            var quoteItem = new QuoteItem();
            quoteItem.Id = id;
            quoteItem.ItemNo = quote.QuoteItems.Count + 1;
            quoteItem.Quote = quote;
            quoteItem.Labour = GetLabour(labour);
            quoteItem.Calibration = GetCalibration(calibration);
            quoteItem.Parts = GetParts(parts);
            quoteItem.Carriage = GetCarriage(carriage);
            quoteItem.Investigation = GetInvestigation(investigation);
            quoteItem.Report = report;
            quoteItem.Days = GetDays(days);
            quoteItem.BeyondEconomicRepair = beyondEconomicRepair;
            quoteItem.Status = _listItemRepository.GetByType(ListItemType.StatusQuotedPrepared);
            ValidateAnnotatedObjectThrowOnFailure(quoteItem);
            var jobItem = GetJobItem(jobItemId);
            jobItem.Status = _listItemRepository.GetByType(ListItemType.StatusQuotedPrepared);
            _jobItemRepository.EmitItemHistory(
                CurrentUser, jobItemId, 0, 0, String.Format("Item quoted on {0}", quote.QuoteNumber), ListItemType.StatusQuotedPrepared, ListItemType.WorkTypeAdministration);
            quoteItem.JobItem = jobItem;
            _quoteItemRepository.Create(quoteItem);
            _jobItemRepository.Update(jobItem);
            return quoteItem;
        }

        public PendingQuoteItem CreatePending(
            Guid id, Guid customerId, Guid jobItemId, decimal labour, decimal calibration, decimal parts, decimal carriage, decimal investigation, string report, int days, bool beyondEconomicRepair, string orderNo, string adviceNo)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            if (id == Guid.Empty)
                throw new ArgumentException("An ID must be supplied for the pending item");
            if (_quoteItemRepository.JobItemHasPendingQuoteItem(jobItemId))
                throw new DomainValidationException(Messages.PendingItemExists, "JobItemId");
            var pendingItem = new PendingQuoteItem();
            pendingItem.Id = id;
            pendingItem.OrderNo = orderNo;
            pendingItem.AdviceNo = adviceNo;
            pendingItem.Customer = GetCustomer(customerId);
            pendingItem.JobItem = GetJobItem(jobItemId);
            pendingItem.Labour = GetLabour(labour);
            pendingItem.Calibration = GetCalibration(calibration);
            pendingItem.Parts = GetParts(parts);
            pendingItem.Carriage = GetCarriage(carriage);
            pendingItem.Investigation = GetInvestigation(investigation);
            pendingItem.Report = report;
            pendingItem.Days = GetDays(days);
            pendingItem.BeyondEconomicRepair = beyondEconomicRepair;
            ValidateAnnotatedObjectThrowOnFailure(pendingItem);
            _quoteItemRepository.CreatePendingQuoteItem(pendingItem);
            return pendingItem;
        }

        public QuoteItem Edit(
            Guid id, decimal labour, decimal calibration, decimal parts, decimal carriage, decimal investigation, string report, int days, bool beyondEconomicRepair)
        {
            if (!CurrentUser.HasRole(UserRole.Admin | UserRole.Manager))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            var quoteItem = GetById(id);
            quoteItem.Labour = GetLabour(labour);
            quoteItem.Calibration = GetCalibration(calibration);
            quoteItem.Parts = GetParts(parts);
            quoteItem.Carriage = GetCarriage(carriage);
            quoteItem.Investigation = GetInvestigation(investigation);
            quoteItem.Report = report;
            quoteItem.Days = GetDays(days);
            quoteItem.BeyondEconomicRepair = beyondEconomicRepair;
            ValidateAnnotatedObjectThrowOnFailure(quoteItem);
            var quote = _quoteRepository.GetById(quoteItem.Quote.Id);
            quote.Revision++;
            _quoteRepository.Update(quote);
            _quoteItemRepository.Update(quoteItem);
            return quoteItem;
        }

        public PendingQuoteItem EditPending(
            Guid pendingItemId, decimal labour, decimal calibration, decimal parts, decimal carriage, decimal investigation, string report, int days, bool beyondEconomicRepair, string orderNo, string adviceNo)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            var pendingItem = _quoteItemRepository.GetPendingQuoteItem(pendingItemId);
            if (pendingItem == null)
                throw new ArgumentException("A valid ID must be supplied for the pending item");
            pendingItem.OrderNo = orderNo;
            pendingItem.AdviceNo = adviceNo;
            pendingItem.Labour = GetLabour(labour);
            pendingItem.Calibration = GetCalibration(calibration);
            pendingItem.Parts = GetParts(parts);
            pendingItem.Carriage = GetCarriage(carriage);
            pendingItem.Investigation = GetInvestigation(investigation);
            pendingItem.Days = GetDays(days);
            pendingItem.Report = report;
            pendingItem.BeyondEconomicRepair = beyondEconomicRepair;
            ValidateAnnotatedObjectThrowOnFailure(pendingItem);
            _quoteItemRepository.UpdatePendingItem(pendingItem);
            return pendingItem;
        }

        public QuoteItem Accept(Guid quoteItemId)
        {
            if (!CurrentUser.HasRole(UserRole.Manager))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            var quoteItem = _quoteItemRepository.GetById(quoteItemId);
            if (quoteItem == null)
                throw new ArgumentException("An invalid quote item ID was supplied");
            if (quoteItem.Status.Type == ListItemType.StatusQuoteAccepted)
                return quoteItem;
            var quoteAcceptedStatus = _listItemRepository.GetByType(ListItemType.StatusQuoteAccepted);
            quoteItem.Status = quoteAcceptedStatus;
            var jobItem = quoteItem.JobItem;
            jobItem.Status = quoteAcceptedStatus;
            _jobItemRepository.EmitItemHistory(
                CurrentUser, jobItem.Id, 0, 0, String.Format("Item accepted on quote {0}", quoteItem.Quote.QuoteNumber), ListItemType.StatusQuoteAccepted, ListItemType.WorkTypeAdministration);
            _jobItemRepository.Update(jobItem);
            _quoteItemRepository.Update(quoteItem);
            return quoteItem;
        }

        public QuoteItem Reject(Guid quoteItemId)
        {
            if (!CurrentUser.HasRole(UserRole.Manager))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            var quoteItem = _quoteItemRepository.GetById(quoteItemId);
            if (quoteItem == null)
                throw new ArgumentException("An invalid quote item ID was supplied");
            if (quoteItem.Status.Type == ListItemType.StatusQuoteRejected)
                return quoteItem;
            var quoteRejectedStatus = _listItemRepository.GetByType(ListItemType.StatusQuoteRejected);
            quoteItem.Status = quoteRejectedStatus;
            var jobItem = quoteItem.JobItem;
            jobItem.Status = quoteRejectedStatus;
            _jobItemRepository.EmitItemHistory(
                CurrentUser, jobItem.Id, 0, 0, String.Format("Item rejected on quote {0}", quoteItem.Quote.QuoteNumber), ListItemType.StatusQuoteRejected, ListItemType.WorkTypeAdministration);
            _jobItemRepository.Update(jobItem);
            _quoteItemRepository.Update(quoteItem);
            return quoteItem;
        }

        public QuoteItem SetQuoted(Guid quoteItemId)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            var quoteItem = _quoteItemRepository.GetById(quoteItemId);
            if (quoteItem == null)
                throw new ArgumentException("An invalid quote item ID was supplied");
            if (quoteItem.Status.Type == ListItemType.StatusQuotedPrepared)
            {
                var quotedStatus = _listItemRepository.GetByType(ListItemType.StatusQuoted);
                quoteItem.Status = quotedStatus;
                var jobItem = quoteItem.JobItem;
                jobItem.Status = quotedStatus;
                _jobItemRepository.EmitItemHistory(
                    CurrentUser, jobItem.Id, 0, 0, String.Format("Quote {0} was printed", quoteItem.Quote.QuoteNumber), ListItemType.StatusQuoted, ListItemType.WorkTypeAdministration);
                _jobItemRepository.Update(jobItem);
                _quoteItemRepository.Update(quoteItem);
            }
            return quoteItem;
        }

        public QuoteItem GetById(Guid id)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            var quoteItem = _quoteItemRepository.GetById(id);
            if (quoteItem == null)
                throw new ArgumentException("A valid ID must be supplied for the quote item");
            return _quoteItemRepository.GetById(id);
        }

        public IEnumerable<QuoteItem> GetQuoteItems(Guid quoteId)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            return _quoteItemRepository.GetQuoteItems(quoteId);
        }

        public int GetQuoteItemsCount(Guid quoteId)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            return _quoteItemRepository.GetQuoteItemsCount(quoteId);
        }

        public IEnumerable<QuoteItem> GetQuoteItemsForJobItem(Guid jobItemId)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            return _quoteItemRepository.GetQuoteItemsForJobItem(jobItemId);
        }

        public PendingQuoteItem GetPendingQuoteItemForJobItem(Guid jobItemId)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            return _quoteItemRepository.GetPendingQuoteItemForJobItem(jobItemId);
        }

        public void DeletePendingQuoteItem(Guid id)
        {
            _quoteItemRepository.DeletePendingQuoteItem(id);
        }

        public int GetPendingQuoteItemsCount()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            return _quoteItemRepository.GetPendingQuoteItemsCount();
        }

        public IEnumerable<PendingQuoteItem> GetPendingQuoteItems()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            return _quoteItemRepository.GetPendingQuoteItems();
        }

        public IEnumerable<PendingQuoteItem> GetPendingQuoteItems(IList<Guid> pendingItemIds)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurity, "CurrentUser");
            return _quoteItemRepository.GetPendingQuoteItems(pendingItemIds);
        }

        private Customer GetCustomer(Guid customerId)
        {
            var customer = _customerRepository.GetById(customerId);
            if (customer == null)
                throw new ArgumentException("A valid ID must be supplied for the customer.");
            return customer;
        }

        private Quote GetQuote(Guid quoteId)
        {
            var quote = _quoteRepository.GetById(quoteId);
            if (quote == null)
                throw new ArgumentException("A valid ID must be supplied for the quote");
            return quote;
        }

        private JobItem GetJobItem(Guid jobItemId)
        {
            var jobItem = _jobItemRepository.GetById(jobItemId);
            if (jobItem == null)
                throw new ArgumentException("A valid ID must be supplied for the job item");
            if (jobItem.Job.IsPending)
                throw new DomainValidationException(Messages.PendingJob, "JobItemId");
            return jobItem;
        }

        private decimal GetLabour(decimal labour)
        {
            if (labour < 0)
                throw new DomainValidationException(Messages.InvalidLabour, "Labour");
            return labour;
        }

        private decimal GetCalibration(decimal calibration)
        {
            if (calibration < 0)
                throw new DomainValidationException(Messages.InvalidCalibration, "Calibration");
            return calibration;
        }

        private decimal GetParts(decimal parts)
        {
            if (parts < 0)
                throw new DomainValidationException(Messages.InvalidParts, "Parts");
            return parts;
        }

        private decimal GetCarriage(decimal carriage)
        {
            if (carriage < 0)
                throw new DomainValidationException(Messages.InvalidCarriage, "Carriage");
            return carriage;
        }

        private decimal GetInvestigation(decimal investigation)
        {
            if (investigation < 0)
                throw new DomainValidationException(Messages.InvalidInvestigation, "Investigation");
            return investigation;
        }

        private int GetDays(int days)
        {
            if (days < 0)
                throw new DomainValidationException(Messages.InvalidDays, "Days");
            return days;
        }
    }
}